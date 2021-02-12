using System.Collections.Generic;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Core.Modules.Ability.Helpers;
using MobaGame.Framework.Types;
using MobaGame.FX;
using UnityEngine;

namespace MobaGame.Entity.Abilities.FlameWitch
{
    /* Unit-Target Spell
        * Applies DOT
        * Deals Damage on Cast
        *
        * When OverHeating;
        *     Enemy heroes in an AOE also recieve DOT
        */


    [CreateAssetMenu(menuName = "Ability/FlameWitch/Ignite")]
    public class Ignite : AbilityObject, IListener<IStepableEvent>, IReflectableAbility
    {
#pragma warning disable 0649
        [SerializeField] private Sprite _icon;
        [Header("Cast Range")] [SerializeField]
        private float _castRange = 5f;

        [Header("Initial Damage")] [SerializeField]
        private float _damage = 100f;


        [Header("Cooldown")] [SerializeField] private float _cooldown;

        [Header("Mana Cost")] [SerializeField] private float _manaCost = 100f;

        [Header("OverHeat FX")] [SerializeField]
        private float _overheatSearchRange = 1f;

        [Header("Damage Over Time")] [SerializeField]
        private float _tickInterval;

        [SerializeField] private int _tickCount;

        [SerializeField] private float _tickDamage;

        public CastRange CastRange { get; set; }

        private Overheat _overheatAbility;

        private List<TickAction> _ticks;

        [SerializeField] private GameObject _igniteFX;
#pragma warning restore 0649

        private void ApplyFX(Transform target, float duration)
        {
            if (_igniteFX == null)
                return;
            var instance = Instantiate(_igniteFX, target.position, Quaternion.identity);
            if (!instance.TryGetComponent<DieAfterDuration>(out var die))
                die = instance.AddComponent<DieAfterDuration>();
            if (!instance.TryGetComponent<FollowTarget>(out var follow))
                follow = instance.AddComponent<FollowTarget>();
            if (instance.TryGetComponent<ParticleSystem>(out var ps))
                ps.Play();
            follow.SetTarget(target);
            die.SetDuration(duration);
            die.StartTimer();
        }

        private bool IsInOverheat => _overheatAbility != null && _overheatAbility.Active;

        public void OnStep(float deltaTime)
        {
            _ticks.AdvanceAllAndRemoveDone(deltaTime);
        }

        public void CastObjectTarget(Actor target)
        {
            //Deal damage
            var damage = new Damage(_damage, DamageType.Magical, DamageFlags.Ability);
//            var targetable = target.GetComponent<ITargetable>();
            var damagable = target.GetModule<IDamageable>();
            damagable.TakeDamage(Self, damage);
            //Gather DOT targets
            var dotTargets = new List<Actor> {target};
            if (IsInOverheat)
            {
                var colliders = Physics.OverlapSphere(target.transform.position, _overheatSearchRange,
                    (int) LayerMaskHelper.Entity);
                foreach (var collider in colliders)
                {
                    if (!AbilityHelper.TryGetActor(collider, out var actor))
                        continue;
                    if(!CheckBuilder.AllowTarget(actor))
                        continue;
                    if (actor == target) //Already added
                        continue;
                    dotTargets.Add(actor);
                }
            }

            foreach (var actor in dotTargets)
            {
                if (!actor.TryGetModule<IDamageable>(out var damageTarget))
                    continue;
                var source = Self;
                var dotDamage = new Damage(_tickDamage, DamageType.Magical, DamageFlags.Ability);

                void internalFunc()
                {
                    damageTarget.TakeDamage(source, dotDamage);
                }

                var tickWrapper = new TickAction
                {
                    Callback = internalFunc,
                    TickCount = _tickCount,
                    TickInterval = _tickInterval
                };

                if (actor.TryGetModule<IKillable>(out var killable))
                    killable.Died += RemoveTick;

                void RemoveTick(object sender, DeathEventArgs args)
                {
                    killable.Died -= RemoveTick;
                    _ticks.Remove(tickWrapper);
                }


                _ticks.Add(tickWrapper);
                ApplyFX(actor.transform, _tickCount * _tickInterval);
            }
        }

        public override void Initialize(Actor data)
        {
            base.Initialize(data);
            Modules.Abilitiable.TryGetAbility(out _overheatAbility);
            
            CheckBuilder = new AbilityPredicateBuilder(data)
            {
                AllowSelf = false,
                Teamable = TeamableChecker.NonAllyOnly(Modules.Teamable),
                Cooldown = new Cooldown(_cooldown),
                MagicCost = new MagicCost(Modules.Magicable, _manaCost),
            };
            View = new SimpleAbilityView
            {
                Cooldown = CheckBuilder.Cooldown,
                StatCost = CheckBuilder.MagicCost,
                Icon = _icon,
            };
            CastRange = new CastRange(data.transform){MaxDistance = _castRange};
            CheckBuilder.RebuildChecks();
            Register(data);
            _ticks = new List<TickAction>();
        }

        public SimpleAbilityView View { get; set; }

        public AbilityPredicateBuilder CheckBuilder{ get; set; }

        public override void ConfirmCast()
        {
            if (!CheckBuilder.AllowCast())
                return;

            if (!AbilityHelper.TryRaycastActor(out var actor))
                return;
            
            if (!CastRange.InRange(actor))
                return;
            
            if (!CheckBuilder.AllowTarget(actor))
                return;

            if (!AbilityHelper.AllowSpellTargets(actor))
                return;

            if (!AbilityHelper.HasModule<IDamageable>(actor.gameObject))
                return;



            CheckBuilder.DoCast();
            CastObjectTarget(actor);
            Modules.Abilitiable.NotifyAbilityCast(new AbilityEventArgs(Self, CheckBuilder.MagicCost.Cost));
        }

        public override IAbilityView GetAbilityView() => View;


        public void Register(IStepableEvent source)
        {
            CheckBuilder.Cooldown.Register(source);
            source.Step += OnStep;
        }


        public void Unregister(IStepableEvent source)
        {
            CheckBuilder.Cooldown.Unregister(source);
            source.Step -= OnStep;
        }

        public void CastReflected(Actor caster)
        {
            throw new System.NotImplementedException();
        }
    }
}