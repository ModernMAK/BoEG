using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Types;
using MobaGame.FX;
using System;
using MobaGame.Framework.Core.Modules.Ability.Helpers;
using UnityEngine;

namespace MobaGame.Entity.Abilities.FlameWitch
{
    /* Transformation Spell
     * Applies DOT around FlameWitch (Melee Range)
     * Upgrades FlameWitch's Abilities.
     * On Death, deal damage to enemies in AOE.
     * Drains mana.
    */
    [CreateAssetMenu(menuName = "Ability/FlameWitch/Overheat")]
    public class Overheat : AbilityObject, IListener<IStepableEvent>, IToggleable
    {
#pragma warning disable 0649
        [SerializeField] public Sprite _icon;
        [SerializeField] public float _damagePerSecond;

        [Header("Damage On Death")] public float _deathRange;
        [SerializeField] public float _deathDamage;

        [Header("Damage Over Time")] [SerializeField]
        public float _dotRange;

        [Header("Mana Cost")] [SerializeField] public float _manaCostPerSecond;

        private TickAction _tickHelper;

        [SerializeField] private GameObject _overheatFX;
        private ParticleSystem _particleSystemInstance;


#pragma warning restore 0649

		public bool Active
        {
            get => View.Toggleable.Active;
            set => View.Toggleable.Active = value;
        }

        event EventHandler<ChangedEventArgs<bool>> IToggleable.Toggled
        {
            add => View.Toggleable.Toggled += value;
            remove => View.Toggleable.Toggled -= value;
        }
        
        private ParticleSystem ApplyFX(Transform target)
        {
            if (_overheatFX == null)
                return null;
            var instance = Instantiate(_overheatFX, target.position, Quaternion.identity);
            if (!instance.TryGetComponent<FollowTarget>(out var follow))
                follow = instance.AddComponent<FollowTarget>();
            if (instance.TryGetComponent<ParticleSystem>(out var ps))
                ps.Stop();
            follow.SetTarget(target);
            return ps;
        }


        public void OnStep(float deltaTime)
        {
            if (Active)
                _tickHelper.Advance(deltaTime);
        }


        public override void Initialize(Actor data)
        {
            base.Initialize(data);
            _tickHelper = new InfiniteTickAction {Callback = OnTick, TickInterval = 1f};
            
            CheckBuilder = new AbilityPredicateBuilder(data)
            {
                Teamable = TeamableChecker.NonAllyOnly(Modules.Teamable),
                MagicCost = new MagicCost(Modules.Magicable,_manaCostPerSecond),
                AllowSelf = false,
            };
            View = new SimpleAbilityView()
            {
                Icon = _icon,
                Cooldown = CheckBuilder.Cooldown,
                StatCost = CheckBuilder.MagicCost,
                Toggleable = new ToggleableAbilityView(){ShowActive = true}
            };
            CheckBuilder.RebuildChecks();
            
            Register(data);
            if (_particleSystemInstance == null)
                _particleSystemInstance = ApplyFX(data.transform);
        }

        public SimpleAbilityView View { get; set; }

        public AbilityPredicateBuilder CheckBuilder { get; set; }

        public void UpdateParticleSystemState()
        {
            if (_particleSystemInstance != null)
                if (Active)
                    _particleSystemInstance.Play();
                else
                    _particleSystemInstance.Stop();
        }

        public override void ConfirmCast()
        {
            Active = !Active;

            UpdateParticleSystemState();

            if (Active)
            {
                OnTick();
            }
        }

        public override IAbilityView GetAbilityView() => View;

        private void OnTick()
        {
            if (Active && CheckBuilder.MagicCost.TrySpendCost())
            {
                var dotTargets =
                    Physics.OverlapSphere(Self.transform.position, _dotRange, (int) LayerMaskHelper.Entity);
                foreach (var col in dotTargets)
                {
                    if (!AbilityHelper.TryGetActor(col, out var actor))
                        continue;
                    if (!CheckBuilder.AllowTarget(actor))
                        continue;
                    if (!actor.TryGetModule<IDamageable>(out var damageTarget))
                        continue;

                    var damage = new Damage(_damagePerSecond, DamageType.Magical, DamageFlags.Ability);
                    damageTarget.TakeDamage(Self, damage);
                }
            }
            else
            {
                Active = false;
                UpdateParticleSystemState();
            }
        }

        public void Register(IStepableEvent source)
        {
            source.Step += OnStep;
        }

        public void Unregister(IStepableEvent source)
        {
            source.Step -= OnStep;
        }
    }
}