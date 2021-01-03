using Framework.Ability;
using Framework.Core;
using Framework.Core.Modules;
using Framework.Types;
using Triggers;
using UnityEngine;

namespace Entity.Abilities.LastHunter
{
    [CreateAssetMenu(menuName = "Ability/LastHunter/HunterStrike")]
    public class HuntersStrike : AbilityObject, IGroundTargetAbility, IObjectTargetAbility<Actor>, ICooldownAbility,
        IStatCostAbility, IListener<IStepableEvent>
    {
        /* Ground target
         * Blink To target
         * Deal damage to units between origin and target
         */
#pragma warning disable 0649
        private const float _pathWidth = 1f;
        [SerializeField] private float _bonusDamage;
        [SerializeField] private float _castRange;
        [SerializeField] private float _manaCost;
        [SerializeField] private float _cooldown;
        private DurationTimer _cooldownHelper;
#pragma warning restore 0649

        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);
            actor.AddSteppable(this);
            _cooldownHelper = new DurationTimer(_cooldown);
            _cooldownHelper.ElapsedTime = _cooldownHelper.Duration;
        }

        public override void ConfirmCast()
        {
            if (!_cooldownHelper.Done)
                return;

            var ray = AbilityHelper.GetScreenRay();
            if (!AbilityHelper.TryGetWorldOrEntity(ray, out var hit))
                return;
            var unitCast = AbilityHelper.TryGetActor(hit.collider, out var actor);
            if(actor == Self)
                return;
            var position = unitCast
                ? actor.transform.position
                : hit.point;
            if (!AbilityHelper.InRange(Self.transform, position, _castRange))
                return;
            if (!AbilityHelper.TrySpendMagic(this, Modules.Magicable))
                return;

            _cooldownHelper.Reset();

            if (unitCast)
                CastObjectTarget(actor);
            else
                CastGroundTarget(position);
            Modules.Abilitiable.NotifySpellCast(new SpellEventArgs() {Caster = Self, ManaSpent = Cost});
        }

        public void CastGroundTarget(Vector3 worldPos)
        {
            var origin = Self.transform.position;
            var length = AbilityHelper.GetLineLength(origin, worldPos);
            var boxHalfExtents = new Vector3(_pathWidth, 2f, length);
            var rotation = AbilityHelper.GetRotation(origin, worldPos);
            var center = AbilityHelper.GetBoxCenter(origin, boxHalfExtents, rotation);

            var colliders = Physics.OverlapBox(center, boxHalfExtents, rotation, (int) LayerMaskHelper.Entity);

            var atkDamage = Modules.Attackerable.AttackDamage;

            var dmg = new Damage(_bonusDamage + atkDamage, DamageType.Physical,
                DamageModifiers.Ability | DamageModifiers.Attack);

            foreach (var col in colliders)
            {
                if (!AbilityHelper.TryGetActor(col, out var actor))
                    continue;
                if (IsSelf(actor))
                    continue;

                if (AbilityHelper.SameTeam(Modules.Teamable, actor))
                    continue;
                if (!actor.TryGetComponent<IDamageTarget>(out var target))
                    continue;

                target.TakeDamage(Self.gameObject, dmg);
            }

            Modules.Movable.WarpTo(worldPos);
            Modules.Commandable.ClearCommands();
        }

        //We jump behind them
        public void CastObjectTarget(Actor target)
        {
            var origin = Self.transform.position;
            var dest = target.transform.position;
            var offset = dest - origin;
            const float JumpDistance = 0.5f;
            CastGroundTarget(dest + offset.normalized * JumpDistance);
        }

        public float Cooldown => _cooldownHelper.Duration;

        public float CooldownRemaining => _cooldownHelper.RemainingTime;

        public float CooldownNormal => _cooldownHelper.ElapsedTime / _cooldownHelper.Duration;
        public float Cost => _manaCost;

        public bool CanSpendCost() => Modules.Magicable.HasMagic(Cost);

        private void UpdateCooldown(float deltaTime)
        {
            if (!_cooldownHelper.Done)
                _cooldownHelper.AdvanceTime(deltaTime);
        }

        public void Register(IStepableEvent source)
        {
            source.PreStep += UpdateCooldown;
        }

        public void Unregister(IStepableEvent source)
        {
            source.PreStep -= UpdateCooldown;
        }
    }
}