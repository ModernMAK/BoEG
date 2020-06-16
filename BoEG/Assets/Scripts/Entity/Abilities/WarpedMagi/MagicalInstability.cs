using Entity.Abilities.FlameWitch;
using Framework.Core;
using Framework.Core.Modules;
using Framework.Types;
using UnityEngine;

namespace Entity.Abilities.WarpedMagi
{
    [CreateAssetMenu(menuName = "Ability/WarpedMagi/MagicalInstability")]
    public class MagicalInstability : AbilityObject, IStepable
    {
        /* Self-Target Spell
         * Negates Magical Damage.
         * Magical Damage grants mana for the duration.
         */
        [Header("Mana Cost")] [SerializeField] private float _manaCost;
        [SerializeField] private float _manaGainPerDamage;
        [SerializeField] private float _duration;
        private float _timeElapsed;

        private IArmorable _armorable;
        private IMagicable _magicable;
        private IAbilitiable _abilitiable;

        private bool _isActive;

        public override void Initialize(Actor actor)
        {
            base.Initialize(actor);
            _armorable = Self.GetComponent<IArmorable>();
            _magicable = Self.GetComponent<IMagicable>();
            _armorable.Resisting += OnResisting;
            _abilitiable = Self.GetComponent<IAbilitiable>();
            actor.AddSteppable(this);
        }

        private void OnResisting(object sender, ArmorableEventArgs e)
        {
            //Dont do anything if we aren't on
            if (!_isActive)
                return;
            //Only apply to magical damage
            if (e.Damage.Type != DamageType.Magical)
                return;
            var fullDamage = e.Damage.Value + e.Reduction;
            var manaGained = _manaGainPerDamage * fullDamage;
            manaGained = Mathf.Max(manaGained, 0f);
            //Gain mana
            _magicable.Magic += manaGained;
            e.Reduction = e.Damage.Value; //Negate all damage, we dont rely on buffs to do the negation
        }

        public override void ConfirmCast()
        {
            CastLogic();
        }

        void CastLogic()
        {
            if (_isActive)
                return;
            if (!_magicable.HasMagic(_manaCost))
                return;
            _magicable.SpendMagic(_manaCost);
            _isActive = true;
            _timeElapsed = 0f;
            _abilitiable.NotifySpellCast(new SpellEventArgs() {Caster = Self, ManaSpent = _manaCost});
        }

        public void PreStep(float deltaTime)
        {
        }


        public void Step(float deltaTime)
        {
        }

        public void PostStep(float deltaTime)
        {
            if (!_isActive)
                return;
            _timeElapsed += deltaTime;
            if (_timeElapsed >= _duration)
            {
                _isActive = false;
            }
        }

        public void PhysicsStep(float deltaTime)
        {
        }
    }
}