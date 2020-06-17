using Framework.Core;
using Framework.Core.Modules;
using Framework.Types;
using Modules.Teamable;
using Triggers;
using UnityEngine;

namespace Entity.Abilities.FlameWitch
{
    [CreateAssetMenu(menuName = "Ability/FlameWitch/FireBall")]
    public class Fireball : AbilityObject
    {
        private Overheat _overheat;

        [Header("Mana")] [SerializeField] private float _manaCost;
        [Header("Damage")] [SerializeField] private float _damage;

        [Header("Cast Range")] [SerializeField]
        private float _castRange;

        [SerializeField] private float _overheatCastRange;

        [SerializeField] private float _pathWidth;


        /* Ground-Taget Spell
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
            CastLogic();
        }

        void CastLogic()
        {
            var ray = AbilityHelper.GetScreenRay();
            if (!Physics.Raycast(ray, out var hit, 100f, (int) LayerMaskHelper.World))
                return;
            _commonAbilityInfo.Range = _overheat.IsActive ? _overheatCastRange : _castRange;
            if (!_commonAbilityInfo.InRange(hit.point))
                return;
            if (!_commonAbilityInfo.TrySpendMana())
                return;


            var t = Self.transform.position;
            var p = hit.point;

            var center = (p + t) / 2f;
            var dir = (p - t);
            var bound = new Vector3(_pathWidth, 2, dir.magnitude);
            var halfbound = bound / 2;
            var rotation = Quaternion.LookRotation(dir);

            var colliders = Physics.OverlapBox(center, halfbound, rotation, (int) LayerMaskHelper.Entity);
            var dmg = new Damage(_damage, DamageType.Magical, DamageModifiers.Ability);

            foreach (var col in colliders)
            {
                if (!col.gameObject.TryGetComponent<Actor>(out var actor))
                    continue;

                if (_commonAbilityInfo.SameTeam(actor.gameObject))
                    continue;

                if (!actor.TryGetComponent<IDamageTarget>(out var damageTarget))
                    continue;

                damageTarget.TakeDamage(Self.gameObject, dmg);
            }

            _commonAbilityInfo.NotifySpellCast();
        }
    }
}