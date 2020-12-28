using System.Collections.Generic;
using Entity.Abilities.FlameWitch;
using Framework.Ability;
using Framework.Core;
using Framework.Core.Modules;
using Framework.Types;
using Modules.Teamable;
using Triggers;
using UnityEngine;

namespace Entity.Abilities.WarpedMagi
{
    [CreateAssetMenu(menuName = "Ability/WarpedMagi/UnstableMagic")]
    public class UnstableMagic : AbilityObject, IGroundTargetAbility, IObjectTargetAbility<Actor>
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
#pragma warning restore 0649

        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);
            _commonAbilityInfo.Range = _castRange;
            _commonAbilityInfo.ManaCost = _manaCost;
        }

        public override void ConfirmCast()
        {
            var ray = AbilityHelper.GetScreenRay();
            if (!Physics.Raycast(ray, out var hit, 100f, (int) (LayerMaskHelper.World | LayerMaskHelper.Entity)))
                return;

            var isGround = !hit.collider.TryGetComponent<Actor>(out var actor);
            if (isGround && !_commonAbilityInfo.InRange(hit.point))
                return;
            if (!isGround && !_commonAbilityInfo.InRange(actor.transform))
                return;

            if (!_commonAbilityInfo.TrySpendMana())
                return;

            if (isGround)
                GroundTarget(hit.point);
            else
                ObjectTarget(actor);
        }

        public void GroundTarget(Vector3 worldPos)
        {
            if (TrySearchTarget(worldPos, out var target))
                InternalJumpTarget(target);
        }

        public void ObjectTarget(Actor target) => InternalJumpTarget(target);

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
        }

        private bool TrySearchTarget(Vector3 area, out Actor target) => TrySearchTarget(area, new Actor[0], out target);

        private bool TrySearchTarget(Vector3 area, ICollection<Actor> ignore, out Actor target)
        {
            var potentialTargets = Physics.OverlapSphere(area, _searchRange, (int) LayerMaskHelper.Entity);
            foreach (var potentialTarget in potentialTargets)
            {
                if (!potentialTarget.TryGetComponent<Actor>(out var actor))
                    continue;
                if (actor == Self)
                    continue;
                if (ignore.Contains(actor))
                    continue;
                if (_commonAbilityInfo.Teamable != null)
                    if (actor.TryGetComponent<ITeamable>(out var teamable))
                        if (_commonAbilityInfo.SameTeam(teamable))
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
    }
}