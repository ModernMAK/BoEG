using System.Collections.Generic;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Types;
using MobaGame.FX;
using UnityEngine;

namespace MobaGame.Entity.Abilities.WarpedMagi
{
    [CreateAssetMenu(menuName = "Ability/WarpedMagi/UnstableMagic")]
    public class UnstableMagic : AbilityObject, IGroundTargetAbility, IObjectTargetAbility<Actor>, IStatCostAbility,
        ICooldownAbility, IListener<IStepableEvent>
    {
        // [Header("Mana Cost")]
        /* Ground Target Spell
         * Magical Nuke, jumps between targets. Dealing less damage per jump.
         * Does not target the same target twice. 
         */

#pragma warning disable 0649

        [SerializeField] private float _manaCost;
        [SerializeField] private float _castRange;
        [Header("Damage")] [SerializeField] private float _damage;
        [Header("Jumps")] [SerializeField] private float _searchRange;
        [SerializeField] private int _additionalJumps;

        [Header("Cooldown")] [SerializeField] private float _cooldown;
        private DurationTimer _cooldownTimer;

        [SerializeField] private GameObject _unstableMagicFX;
#pragma warning restore 0649

        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);
            Register(actor);
            _cooldownTimer = new DurationTimer(_cooldown, true);
        }

        public override void ConfirmCast()
        {
            if (!_cooldownTimer.Done)
                return;
            var ray = AbilityHelper.GetScreenRay();
            if (!AbilityHelper.TryGetWorldOrEntity(ray, out var hit))
                return;

            var unitTarget = AbilityHelper.TryGetActor(hit.collider, out var actor);
            if (actor == Self)
                return;

            if (!unitTarget && !AbilityHelper.InRange(Self.transform, hit.point, _castRange))
                return;
            if (unitTarget && !AbilityHelper.InRange(Self.transform, actor.transform, _castRange))
                return;

            if (!AbilityHelper.TrySpendMagic(this, Modules.Magicable))
                return;


            _cooldownTimer.Reset();
            if (!unitTarget)
                CastGroundTarget(hit.point);
            else
                CastObjectTarget(actor);

            Modules.Abilitiable.NotifySpellCast(new SpellEventArgs() {Caster = Self, ManaSpent = Cost});
        }

        public void CastGroundTarget(Vector3 worldPos)
        {
            if (TrySearchTarget(worldPos, out var target))
                InternalJumpTarget(target);
        }

        public void CastObjectTarget(Actor target) => InternalJumpTarget(target);

        private void InternalJumpTarget(Actor initialTarget)
        {
            InternalSingleTarget(initialTarget);
            var ignore = new List<Actor>() {initialTarget};
            var prevTarget = initialTarget;
            for (var i = 0; i < _additionalJumps; i++)
            {
                if (!TrySearchTarget(prevTarget.transform.position, ignore, out var nextTarget))
                    break;
                InternalSingleTarget(nextTarget);
                prevTarget = nextTarget;
                ignore.Add(prevTarget);
            }
        }

        private void InternalSingleTarget(Actor target)
        {
            var damage = new Damage(_damage, DamageType.Magical, DamageModifiers.Ability);
            //TODO add targetable info
            // var targetable = target.GetComponent<Targetable>();
            // targetable.AffectSpell();
            var damageable = target.GetComponent<IDamageTarget>();
            damageable.TakeDamage(target.gameObject, damage);
            ApplyFX(target.transform);
        }

        private void ApplyFX(Transform target)
        {
            if (_unstableMagicFX == null)
                return;
            var instance = Instantiate(_unstableMagicFX, target.position, Quaternion.identity);
            if (!instance.TryGetComponent<FollowTarget>(out var follow))
                follow = instance.AddComponent<FollowTarget>();
            if (!instance.TryGetComponent<DieAfterParticleCompletion>(out var die))
                die = instance.AddComponent<DieAfterParticleCompletion>();
            if (instance.TryGetComponent<ParticleSystem>(out var ps))
                ps.Play();

            follow.SetTarget(target);
            die.AllowDeath();
        }

        private bool TrySearchTarget(Vector3 area, out Actor target) => TrySearchTarget(area, new Actor[0], out target);

        private bool TrySearchTarget(Vector3 area, ICollection<Actor> ignore, out Actor target)
        {
            var potentialTargets = Physics.OverlapSphere(area, _searchRange, (int) LayerMaskHelper.Entity);
            foreach (var potentialTarget in potentialTargets)
            {
                if (!AbilityHelper.TryGetActor(potentialTarget, out var actor))
                    continue;
                if (IsSelf(actor))
                    continue;
                if (ignore.Contains(actor))
                    continue;
                if (AbilityHelper.SameTeam(Modules.Teamable, actor))
                    continue;

                //TODO when targetable works, add this
                // if (!actor.TryGetComponent<ITargetable>(out _))
                // continue;
                if (!actor.TryGetComponent<IDamageTarget>(out _))
                    continue;

                target = actor;
                return true;
            }

            target = default;
            return false;
        }

        public float Cost => _manaCost;

        public bool CanSpendCost() => Modules.Magicable.HasMagic(Cost);

        public float Cooldown => _cooldownTimer.Duration;
        public float CooldownRemaining => _cooldownTimer.RemainingTime;
        public float CooldownNormal => _cooldownTimer.ElapsedTime / _cooldownTimer.Duration;

        public void OnStep(float deltaTime)
        {
            _cooldownTimer.AdvanceTimeIfNotDone(deltaTime);
        }

        public void Register(IStepableEvent source)
        {
            source.PreStep += OnStep;
        }

        public void Unregister(IStepableEvent source)
        {
            source.PreStep -= OnStep;
        }
    }
}