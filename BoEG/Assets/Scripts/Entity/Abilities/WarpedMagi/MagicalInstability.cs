using Entity.Abilities.FlameWitch;
using Framework.Ability;
using Framework.Core;
using Framework.Core.Modules;
using Framework.Types;
using UnityEngine;

namespace Entity.Abilities.WarpedMagi
{
    [CreateAssetMenu(menuName = "Ability/WarpedMagi/MagicalInstability")]
    public class MagicalInstability : AbilityObject, IListener<IStepableEvent>, INoTargetAbility
    {
        /* Self-Target Spell
         * Negates Magical Damage.
         * Magical Damage grants mana for the duration.
         */
#pragma warning disable 0649

        [Header("Mana Cost")] [SerializeField] private float _manaCost;
        [SerializeField] private float _manaGainPerDamage;
        [SerializeField] private float _duration;
        private float _timeElapsed;

        private IArmorable _armorable;
        // private IMagicable _magicable;
        // private IAbilitiable _abilitiable;

        private bool _isActive;
#pragma warning restore 0649

        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);
            _armorable = Self.GetComponent<IArmorable>();
            // _magicable = Self.GetComponent<IMagicable>();
            _armorable.Resisting += OnResisting;

            // _abilitiable = Self.GetComponent<IAbilitiable>();
            actor.AddSteppable(this);
        }

        private void OnResisting(object sender, ArmorableEventArgs e)
        {
            //Dont do anything if we aren't on
            if (!_isActive)
                return;

            var dmg = e.OutgoingDamage;
            //Only apply to magical damage
            if (dmg.Type != DamageType.Magical)
                return;

            var magicable = _commonAbilityInfo.Magicable;

            //Mana Available (To Gain)
            var manaAvailable = magicable.MagicCapacity - magicable.Magic;
            //Calcualte Potential Block
            var availableDamageBlock = manaAvailable / _manaGainPerDamage;
            //Calculate Block
            var appliedDamageBlock = Mathf.Min(e.OutgoingDamage.Value, availableDamageBlock);
            //Calculate mana to gain
            var manaGained = appliedDamageBlock * _manaGainPerDamage;

            //TODO consider adding a check to ensure manaGained is never negative
            //Gain mana
            magicable.Magic += manaGained;
            //Negate damage damage, we don't rely on buffs to do the negation
            e.OutgoingDamage = dmg.ModifyValue(-appliedDamageBlock);
        }

        public override void ConfirmCast()
        {
            if (_isActive)
                return;
            if (!_commonAbilityInfo.TrySpendMana())
                return;
            NoTarget();
        }

        public void NoTarget()
        {
            _isActive = true;
            _timeElapsed = 0f;
            _commonAbilityInfo.NotifySpellCast();
        }
        
        public void OnPostStep(float deltaTime)
        {
            if (!_isActive)
                return;
            _timeElapsed += deltaTime;
            if (_timeElapsed >= _duration)
            {
                _isActive = false;
            }
        }

        public void Register(IStepableEvent source)
        {
            source.PostStep += OnPostStep;
        }

        public void Unregister(IStepableEvent source)
        {
            source.PostStep -= OnPostStep;
        }
    }
}