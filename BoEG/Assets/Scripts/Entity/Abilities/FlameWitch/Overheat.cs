using Framework.Ability;
using Framework.Core;
using Framework.Core.Modules;
using Framework.Types;
using Modules.Teamable;
using Triggers;
using UnityEngine;

namespace Entity.Abilities.FlameWitch
{
    /* Transformation Spell
     * Applies DOT around FlameWitch (Melee Range)
     * Upgrades FlameWitch's Abilities.
     * On Death, deal damage to enemies in AOE.
     * Drains mana.
    */
    [CreateAssetMenu(menuName = "Ability/FlameWitch/Overheat")]
    public class Overheat : AbilityObject, IListener<IStepableEvent>, INoTargetAbility
    {
#pragma warning disable 0649
        [SerializeField] public float _damagePerSecond;

        [Header("Damage On Death")] public float _deathRange;
        [SerializeField] public float _deathDamage;

        [Header("Damage Over Time")] [SerializeField]
        public float _dotRange;

        [SerializeField] public bool _isActive;
        [Header("Mana Cost")] [SerializeField] public float _manaCostPerSecond;

        private TickAction _tickHelper;

        [SerializeField] private GameObject _overheatFX;
        private ParticleSystem _particleSystemInstance;
#pragma warning restore 0649

        private ParticleSystem ApplyFX(Transform target)
        {
            if (_overheatFX == null)
                return null;
            var instance = Instantiate(_overheatFX, target.position, Quaternion.identity);
            if (!instance.TryGetComponent<FollowTarget>(out var follow))
                follow = instance.AddComponent<FollowTarget>();
            if(instance.TryGetComponent<ParticleSystem>(out var ps))
                ps.Stop();
            follow.SetTarget(target);
            return ps;
        }

        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }


        public void OnStep(float deltaTime)
        {
            if (IsActive)
                _tickHelper.Advance(deltaTime);
        }


        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);
            _tickHelper = new InfiniteTickAction {Callback = OnTick, TickInterval = 1f};
            _commonAbilityInfo.ManaCost = _manaCostPerSecond;
            actor.AddSteppable(this);
            if (_particleSystemInstance == null)
                _particleSystemInstance = ApplyFX(actor.transform);
        }

        public override void ConfirmCast()
        {
            CastNoTarget();
        }

        public void UpdateParticleSystemState()
        {
            if (_particleSystemInstance != null)
                if (IsActive)
                    _particleSystemInstance.Play();
                else
                    _particleSystemInstance.Stop();
        }

        public void CastNoTarget()
        {
            IsActive = !IsActive;

            UpdateParticleSystemState();

            if (IsActive)
            {
                //Immediately start ticking
                OnTick();
            }


            //Do not notify as a spell cast
        }

        private void OnTick()
        {
            if (IsActive && _commonAbilityInfo.TrySpendMana())
            {
                var dotTargets =
                    Physics.OverlapSphere(Self.transform.position, _dotRange, (int) LayerMaskHelper.Entity);
                foreach (var col in dotTargets)
                {
                    if (!AbilityHelper.TryGetActor(col, out var actor))
                        continue;
                    if (IsSelf(actor))
                        continue;
                    if (!actor.TryGetComponent<IDamageTarget>(out var damageTarget))
                        continue;
                    if (_commonAbilityInfo.SameTeam(actor.gameObject))
                        continue;

                    var damage = new Damage(_damagePerSecond, DamageType.Magical, DamageModifiers.Ability);
                    damageTarget.TakeDamage(Self.gameObject, damage);
                }
            }
            else
            {
                IsActive = false;
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