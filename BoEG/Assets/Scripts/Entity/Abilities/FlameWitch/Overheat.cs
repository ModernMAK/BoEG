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
    public class Overheat : AbilityObject, IStepable
    {
        [SerializeField] public float _damagePerSecond;
        [SerializeField] public float _deathDamage;

        [Header("Damage On Death")] public float _deathRange;

        [Header("Damage Over Time")] [SerializeField]
        public float _dotRange;

        [SerializeField] public bool _isActive;
        [Header("Mana Cost")] [SerializeField] public float _manaCostPerSecond;

        private IMagicable _magicable;
        private ITeamable _teamable;
        private TickAction _tickHelper;
        private IAbilitiable _abilitiable;

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
            _magicable = actor.GetComponent<IMagicable>();
            _teamable = actor.GetComponent<ITeamable>();
            _tickHelper = new InfiniteTickAction {Callback = OnTick, TickInterval = 1f};
            actor.AddSteppable(this);
            _abilitiable = Self.GetComponent<IAbilitiable>();
        }

        public override void ConfirmCast()
        {
            IsActive = !IsActive;
            //Immediately start ticking
            if (IsActive)
            {
                OnTick();
                _abilitiable.NotifySpellCast(new SpellEventArgs() {Caster = Self, ManaSpent = _manaCostPerSecond});
            }
        }

        private void OnTick()
        {
            if (IsActive && _magicable.HasMagic(_manaCostPerSecond))
            {
                _magicable.SpendMagic(_manaCostPerSecond);
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
                    if (_teamable != null && actor.TryGetComponent<ITeamable>(out var teamable))
                    {
                        if (_teamable.SameTeam(teamable))
                            continue;
                    }

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