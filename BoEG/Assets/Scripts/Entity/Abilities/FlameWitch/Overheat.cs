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
    public class Overheat : AbilityObject, IStepable, INoTargetAbility
    {
        [SerializeField] public float _damagePerSecond;
        [SerializeField] public float _deathDamage;

        [Header("Damage On Death")] public float _deathRange;

        [Header("Damage Over Time")] [SerializeField]
        public float _dotRange;

        [SerializeField] public bool _isActive;
        [Header("Mana Cost")] [SerializeField] public float _manaCostPerSecond;

        private Framework.Ability.TickAction _tickHelper;
        // private IMagicable _magicable;
        // private ITeamable _teamable;
        // private IAbilitiable _abilitiable;

        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        public void PreStep(float deltaTime)
        {
        }

        public void Step(float deltaTime)
        {
            if (IsActive)
                _tickHelper.Advance(deltaTime);
        }

        public void PostStep(float deltaTime)
        {
        }

        public void PhysicsStep(float deltaTime)
        {
        }

        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);
            _tickHelper = new InfiniteTickAction {Callback = OnTick, TickInterval = 1f};
            _commonAbilityInfo.ManaCost = _manaCostPerSecond;
            actor.AddSteppable(this);
        }

        public override void ConfirmCast()
        {
            NoTarget();
        }

        public void NoTarget()
        {
            IsActive = !IsActive;

            if (IsActive)
                //Immediately start ticking
                OnTick();

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
            }
        }
    }
}