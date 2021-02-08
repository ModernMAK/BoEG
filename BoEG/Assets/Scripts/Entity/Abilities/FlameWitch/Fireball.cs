using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Entity.Abilities.FlameWitch
{
    [CreateAssetMenu(menuName = "Ability/FlameWitch/FireBall")]
    public class Fireball : AbilityObject, IGroundTargetAbility, IStatCostAbility, ICooldownAbility,
        IListener<IStepableEvent>
    {
#pragma warning disable 0649
        private Overheat _overheat;
        [Header("Mana")] [SerializeField] private float _manaCost;
        [Header("Damage")] [SerializeField] private float _damage;

        [Header("Cooldown")] [SerializeField] private float _cooldown;
        private DurationTimer _cooldownTimer;

        [Header("Cast Range")] [SerializeField]
        private float _castRange;

        [SerializeField] private float _overheatCastRange;
        [SerializeField] private float _pathWidth;
#pragma warning restore 0649

        private float CurrentCastRange => _overheat.Active ? _overheatCastRange : _castRange;

        /* Ground-Target Spell
         * Deals damage along path.
         * When OverHeating;
         *     Path is longer.
         */
        public override void Initialize(Actor data)
        {
            base.Initialize(data);
            _cooldownTimer = new DurationTimer(_cooldown,true);
            Modules.Abilitiable.TryGetAbility(out _overheat);
            Register(data);
        }

        public override void ConfirmCast()
        {
            if (!_cooldownTimer.Done)
                return;
            var ray = AbilityHelper.GetScreenRay();
            if (!AbilityHelper.TryGetWorld(ray, out var hit))
                return;
            var range = _overheat.IsActive ? _overheatCastRange : _castRange;
            if (!AbilityHelper.InRange(Self.transform, hit.point, range))
                return;
            if (!AbilityHelper.TrySpendMagic(this, Modules.Magicable))
                return;

            _cooldownTimer.Reset();
            CastGroundTarget(hit.point);
            Modules.Abilitiable.NotifyAbilityCast(new AbilityEventArgs(Self, Cost));
        }

        public void CastGroundTarget(Vector3 worldPos)
        {
            var origin = Self.transform.position;
            var boxBaseSize = new Vector2(_pathWidth, 2);
            var length = AbilityHelper.GetLineLength(origin, worldPos);
            length = Mathf.Min(length, CurrentCastRange);
            var boxHalfExtents = new Vector3(boxBaseSize.x, boxBaseSize.y, length) / 2;
            var rotation = AbilityHelper.GetRotation(origin, worldPos);
            var center = AbilityHelper.GetBoxCenter(origin, boxHalfExtents, rotation);
            var colliders = Physics.OverlapBox(center, boxHalfExtents, rotation, (int) LayerMaskHelper.Entity);
            var dmg = new Damage(_damage, DamageType.Magical, DamageModifiers.Ability);

            foreach (var col in colliders)
            {
                if (!AbilityHelper.TryGetActor(col, out var actor))
                    continue; //Not an actor, ignore

                if (IsSelf(actor))
                    continue; //Always ignore self

                if (AbilityHelper.SameTeam(Modules.Teamable, actor))
                    continue; //Ignore if allies

                if (!actor.TryGetModule<IDamageable>(out var damageTarget))
                    continue; //Ignore if cant damage

                damageTarget.TakeDamage(Self, dmg);
            }
        }

        public float Cost => _manaCost;

        public bool CanSpendCost() => Modules.Magicable.HasMagic(Cost);

        public float Cooldown => _cooldownTimer.Duration;

        public float CooldownRemaining => _cooldownTimer.RemainingTime;

        public float CooldownNormal => _cooldownTimer.ElapsedTime / _cooldownTimer.Duration;

        public void Register(IStepableEvent source)
        {
            source.PreStep += OnPreStep;
        }

        private void OnPreStep(float deltaTime)
        {
            _cooldownTimer.AdvanceTimeIfNotDone(deltaTime);
        }

        public void Unregister(IStepableEvent source)
        {
            source.PreStep += OnPreStep;
        }
    }
}