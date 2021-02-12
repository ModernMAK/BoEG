using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Core.Modules.Ability.Helpers;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Entity.Abilities.WarpedMagi
{
    [CreateAssetMenu(menuName = "Ability/WarpedMagi/MagicalInstability")]
    public class MagicalInstability : AbilityObject, IListener<IStepableEvent>
    {
        /* Self-Target Spell
         * Negates Magical Damage.
         * Magical Damage grants mana for the duration.
         */
#pragma warning disable 0649

        [Header("Mana Cost")] [SerializeField] private float _manaCost;
        [SerializeField] private float _manaGainPerDamage;
        [SerializeField] private float _duration;
        private DurationTimer _activeTimer;
        [Header("Cooldown")] private float _cooldown;

        private IArmorable _armorable;

        public AbilityPredicateBuilder Checker { get; set; }
        public SimpleAbilityView View { get; set; }
        public override IAbilityView GetAbilityView() => View;
#pragma warning restore 0649

		public override void Initialize(Actor data)
        {
            base.Initialize(data);
            _activeTimer = new DurationTimer(_duration);
            _armorable = Self.GetModule<IArmorable>();
            Checker = new AbilityPredicateBuilder(data)
            {
                Cooldown = new Cooldown(_cooldown),
                MagicCost = new MagicCost(Modules.Magicable, _manaCost)
            };
            View = new SimpleAbilityView()
            {
                Cooldown = Checker.Cooldown,
                StatCost = Checker.MagicCost,
                Toggleable = new ToggleableAbilityView()
                {
                    ShowActive = true
                }
            };
            
            Register(data);
        }

        private void OnDamageMitigation(object sender, ChangableEventArgs<SourcedDamage> e)
        {
            //Dont do anything if we aren't on
            if (!View.Toggleable.Active)
                return;

            var dmg = e.After.Value;
            //Only apply to magical damage
            if (dmg.Type != DamageType.Magical)
                return;

            var magicable = Modules.Magicable;

            //Mana Available (To Gain)
            var manaAvailable = magicable.Capacity.Total - magicable.Value;
            //Calcualte Potential Block
            var availableDamageBlock = manaAvailable / _manaGainPerDamage;
            //Calculate Block
            var appliedDamageBlock = Mathf.Min(dmg.Value, availableDamageBlock);
            //Calculate mana to gain
            var manaGained = appliedDamageBlock * _manaGainPerDamage;

            //Gain mana
            magicable.Value += manaGained;
            //Negate damage damage, we don't rely on buffs to do the negation
            dmg = dmg.ModifyValue(-appliedDamageBlock);
            e.After = e.After.SetDamage(dmg);
        }

        public override void ConfirmCast()
        {
            if (View.Toggleable.Active)
                return;
            if(!Checker.AllowCast())
                return;
            View.Toggleable.Active = true;
            CastNoTarget();
            Checker.DoCast();
            Checker.Cooldown.End();
            Modules.Abilitiable.NotifyAbilityCast(new AbilityEventArgs(Self, View.StatCost.Cost));
        }


        public void CastNoTarget()
        {
            _armorable.DamageMitigation += OnDamageMitigation;
        }

        public void OnPreStep(float deltaTime)
        {
            if(!View.Toggleable.Active)
                Checker.Cooldown.Advance(deltaTime); //Advance first, otherwise cooldown is started too early
            else if (_activeTimer.AdvanceTimeIfNotDone(deltaTime))
            {
                View.Toggleable.Active = false;
                Checker.Cooldown.Begin();
                _armorable.DamageMitigation -= OnDamageMitigation;
            }
        }

        public void Register(IStepableEvent source)
        {
            source.PreStep += OnPreStep;
        }

        public void Unregister(IStepableEvent source)
        {
            source.PreStep -= OnPreStep;
        }

    }
}