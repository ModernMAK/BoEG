using Framework.Core;
using Framework.Core.Modules;
using Framework.Types;
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
    public class Overheat : AbilityObject, IStepable
    {
        [Header("Mana Cost")] [SerializeField] public float _manaCostPerSecond;

        [Header("Damage Over Time")] [SerializeField]
        public float _dotRange;

        [SerializeField] public float _damagePerSecond;

        [Header("Damage On Death")] public float _deathRange;
        [SerializeField] public float _deathDamage;

        [SerializeField] public bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        private ManaHelper _manaHelper;
        private TickAction _tickHelper;

        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);
            _manaHelper = new ManaHelper() {Magicable = actor.GetComponent<IMagicable>()};
            _tickHelper = new InfiniteTickAction() {Callback = OnTick, TickInterval = 1f};
            actor.AddSteppable(this);
        }

        public override void ConfirmCast()
        {
            IsActive = !IsActive;
            //Immediately start ticking
            if (IsActive)
                OnTick();
        }

        private void OnTick()
        {
            if (IsActive && _manaHelper.CanSpendMana(_manaCostPerSecond))
            {
                _manaHelper.SpendMana(_manaCostPerSecond);
                var dotTargets =
                    Physics.OverlapSphere(Self.transform.position, _dotRange, (int) LayerMaskHelper.Entity);
                foreach (var col in dotTargets)
                {
                    if (!col.gameObject.TryGetComponent<Actor>(out var actor))
                        continue;
                    if (actor == Self)
                        continue;
                    if (!actor.TryGetComponent<IDamageTarget>(out var damageTarget))
                        continue;
                    var damage = new Damage(_damagePerSecond, DamageType.Magical, DamageModifiers.Ability);
                    damageTarget.TakeDamage(Self.gameObject, damage);
                }
            }
            else IsActive = false;
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
    }
}