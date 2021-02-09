using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Entity.Abilities.WarpedMagi
{
    [CreateAssetMenu(menuName = "Ability/WarpedMagi/MagicalInstability")]
    public class MagicalInstability : AbilityObject, IListener<IStepableEvent>, INoTargetAbility, IStatCostAbility,
        ICooldownAbility, IToggleableAbility
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
        private DurationTimer _cooldownTimer;

        private IArmorable _armorable;

        private bool _isActive;
#pragma warning restore 0649

        public override void Initialize(Actor data)
        {
            base.Initialize(data);
            _activeTimer = new DurationTimer(_duration);
            _cooldownTimer = new DurationTimer(_cooldown, true);
            _armorable = Self.GetModule<IArmorable>();
            Register(data);
        }

        private void OnDamageMitigation(object sender, ChangableEventArgs<SourcedDamage> e)
        {
            //Dont do anything if we aren't on
            if (!_isActive)
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
            if (Active)
                return;
            if (!_cooldownTimer.Done)
                return;

            if (!AbilityHelper.TrySpendMagic(this, Modules.Magicable))
                return;
            _isActive = true;
            CastNoTarget();
            _activeTimer.Reset();
            Modules.Abilitiable.NotifyAbilityCast(new AbilityEventArgs(Self, Cost));
        }


        public void CastNoTarget()
        {
            _armorable.DamageMitigation += OnDamageMitigation;
        }

        public void OnPreStep(float deltaTime)
        {
            _cooldownTimer.AdvanceTimeIfNotDone(deltaTime); //Advance first, otherwise cooldown is started too early
            if (_activeTimer.AdvanceTimeIfNotDone(deltaTime))
            {
                _isActive = false;
                _cooldownTimer.Reset();
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

        public float Cost => _manaCost;

        public bool CanSpendCost() => Modules.Magicable.HasMagic(Cost);

        public float Cooldown => _cooldownTimer.Duration;
        public float CooldownRemaining => _cooldownTimer.RemainingTime;
        public float CooldownNormal => _cooldownTimer.ElapsedTime / _cooldownTimer.Duration;

        public bool Active => _isActive;
    }
}