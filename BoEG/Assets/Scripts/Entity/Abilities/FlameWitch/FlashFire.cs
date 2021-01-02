using System;
using Framework.Ability;
using Framework.Core;
using Framework.Core.Modules;
using Framework.Types;
using Modules.Teamable;
using Triggers;
using UnityEngine;

namespace Entity.Abilities.FlameWitch
{
    [CreateAssetMenu(menuName = "Ability/FlameWitch/FlashFire")]
    public class FlashFire : AbilityObject, INoTargetAbility, IStatCostAbility
    {
        /* Channeling Spell
         * Deals damage in an AOE around FlameWitch
         * Travels further based on channel duration.
         * When Overheating;
         *     Channel does not prevent movement.
         */

        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);
            Modules.Abilitiable.FindAbility<Overheat>(out _overheatAbility);
        }

#pragma warning disable 0649
        [Header("Channel Time")] [SerializeField]
        private float _channelTime;

        [Header("Mana Cost")] [SerializeField] private float _manaCost;

        [Header("Area Of Effect")] [SerializeField]
        private float _aoeSearchRange;

        [SerializeField] private float _aoeDamage;


        private Overheat _overheatAbility;
#pragma warning restore 0649


        public override void ConfirmCast()
        {
            if (Modules.Magicable.HasMagic(Cost))
                return;

            CastNoTarget();
        }

        public void CastNoTarget()
        {
            var targets = Physics.OverlapSphere(Self.transform.position, _aoeSearchRange, (int) LayerMaskHelper.Entity);

            var damage = new Damage(_aoeDamage, DamageType.Magical, DamageModifiers.Ability);
            foreach (var target in targets)
            {
                if (!target.TryGetComponent<Actor>(out var actor))
                    continue;
                if (actor == Self)
                    continue;

                if (!target.TryGetComponent<IDamageTarget>(out var damageTarget))
                    continue;
                if (AbilityHelper.SameTeam(Modules.Teamable, target))
                    continue;

                damageTarget.TakeDamage(Self.gameObject, damage);
            }

            Modules.Abilitiable.NotifySpellCast(new SpellEventArgs() {Caster = Self, ManaSpent = Cost});
        }

        public float Cost => _manaCost;

        public bool CanSpendCost() => Modules.Magicable.HasMagic(Cost);
    }
}