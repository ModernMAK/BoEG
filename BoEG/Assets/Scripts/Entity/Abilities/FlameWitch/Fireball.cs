﻿using Framework.Ability;
using Framework.Core;
using Framework.Core.Modules;
using Framework.Types;
using Modules.Teamable;
using Triggers;
using UnityEngine;

namespace Entity.Abilities.FlameWitch
{
    [CreateAssetMenu(menuName = "Ability/FlameWitch/FireBall")]
    public class Fireball : AbilityObject, IGroundTargetAbility
    {
#pragma warning disable 0649
        private Overheat _overheat;
        [Header("Mana")] [SerializeField] private float _manaCost;
        [Header("Damage")] [SerializeField] private float _damage;

        [Header("Cast Range")] [SerializeField]
        private float _castRange;

        [SerializeField] private float _overheatCastRange;
        [SerializeField] private float _pathWidth;
#pragma warning restore 0649


        /* Ground-Target Spell
         * Deals damage along path.
         * When OverHeating;
         *     Path is longer.
         */
        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);
            _commonAbilityInfo.Abilitiable.FindAbility(out _overheat);
            _commonAbilityInfo.ManaCost = _manaCost;
        }

        public override void ConfirmCast()
        {
            var ray = AbilityHelper.GetScreenRay();
            if (!AbilityHelper.TryGetWorld(ray, out var hit))
                return;
            _commonAbilityInfo.Range = _overheat.IsActive ? _overheatCastRange : _castRange;
            if (!_commonAbilityInfo.InRange(hit.point))
                return;
            if (!_commonAbilityInfo.TrySpendMana())
                return;

            GroundTarget(hit.point);
        }


        public void GroundTarget(Vector3 worldPos)
        {
            var t = Self.transform.position;
            var p = worldPos;

            var dir = (p - t);
            var travelDistance = (_overheat.IsActive ? _overheatCastRange : _castRange);
            var center = t + dir.normalized * travelDistance / 2f;
            var bound = new Vector3(_pathWidth, 2, travelDistance);
            var halfBound = bound / 2;
            var rotation = Quaternion.LookRotation(dir);
            var colliders = Physics.OverlapBox(center, halfBound, rotation, (int) LayerMaskHelper.Entity);
            var dmg = new Damage(_damage, DamageType.Magical, DamageModifiers.Ability);

            foreach (var col in colliders)
            {
                if (!AbilityHelper.TryGetActor(col, out var actor))
                    continue; //Not an actor, ignore

                if (IsSelf(actor))
                    continue; //Always ignore self

                if (_commonAbilityInfo.SameTeam(actor.gameObject))
                    continue; //Ignore if allies

                if (!actor.TryGetComponent<IDamageTarget>(out var damageTarget))
                    continue; //Ignore if cant damage

                damageTarget.TakeDamage(Self.gameObject, dmg);
            }

            _commonAbilityInfo.NotifySpellCast();
        }
    }
}