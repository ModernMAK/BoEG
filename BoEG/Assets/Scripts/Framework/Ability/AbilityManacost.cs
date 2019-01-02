using Framework.Core.Modules;
using UnityEngine;

namespace Framework.Ability.Hero.WarpedMagi
{
    public class AbilityManacost
    {
        public AbilityManacost(IMagicable magicable, float manaCost = 0f)
        {
            _magicable = magicable;
            ManaCost = manaCost;
        }

        public AbilityManacost(GameObject gameObject, float manaCost = 0f) : this(gameObject.GetComponent<IMagicable>(),
            manaCost)
        {
        }

        public AbilityManacost(Component component, float manaCost = 0f) : this(component.gameObject, manaCost)
        {
        }

        private IMagicable _magicable;
        public float ManaCost { get; private set; }

        public void SetManaCost(float manaCost)
        {
            ManaCost = manaCost;
        }

        public void SpendMana()
        {
            _magicable.ModifyMagic(-ManaCost);
        }

        public bool HasMana
        {
            get { return _magicable.Magic.Points >= ManaCost; }
        }
    }
}