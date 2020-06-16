using Framework.Core;
using Framework.Core.Modules;
using Framework.Types;
using Modules.Teamable;
using Triggers;
using UnityEngine;

namespace Entity.Abilities.FlameWitch
{
    [CreateAssetMenu(menuName = "Ability/FlameWitch/FlashFire")]
    public class FlashFire : AbilityObject
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
            Magicable = actor.GetComponent<IMagicable>();
            Teamable = actor.GetComponent<ITeamable>();
            _abilitiable = Self.GetComponent<IAbilitiable>();
            _abilitiable.FindAbility<Overheat>(out _overheatAbility);
        }

        [Header("Channel Time")] [SerializeField]
        private float _channelTime;

        [Header("Mana Cost")] [SerializeField] private float _manaCost;

        [Header("Area Of Effect")] [SerializeField]
        private float _aoeSearchRange;

        [SerializeField] private float _aoeDamage;

        private IAbilitiable _abilitiable;

        private Overheat _overheatAbility;
        private IMagicable Magicable { get; set; }


        private ITeamable Teamable { get; set; }

        public override void ConfirmCast()
        {
            if (!Magicable.HasMagic(_manaCost))
                return;
            Magicable.SpendMagic(_manaCost);
            CastLogic();
        }

        private void CastLogic()
        {
            var targets = Physics.OverlapSphere(Self.transform.position, _aoeSearchRange, (int) LayerMaskHelper.Entity);

            var damage = new Damage(_aoeDamage, DamageType.Magical, DamageModifiers.Ability);
            foreach (var target in targets)
            {
                if (!target.TryGetComponent<Actor>(out var actor))
                    continue;
                if (!target.TryGetComponent<IDamageTarget>(out var damageTarget))
                    continue;
                if (Teamable != null)
                {
                    if (!target.TryGetComponent<ITeamable>(out var teamable))
                        continue;
                    if (Teamable.SameTeam(teamable))
                        continue;
                }

                damageTarget.TakeDamage(Self.gameObject, damage);
            }

            _abilitiable.NotifySpellCast(new SpellEventArgs() {Caster = Self, ManaSpent = _manaCost});
        }
    }
}