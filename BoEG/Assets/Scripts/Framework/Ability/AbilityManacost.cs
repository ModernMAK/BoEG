using Framework.Core.Modules;
using UnityEngine;

namespace Framework.Ability.Hero.WarpedMagi
{
    public class AbilityManacost
    {
        public AbilityManacost(float manaCost = 0f)
        {
            ManaCost = manaCost;
        }


        public float ManaCost { get; private set; }

        public void SetManaCost(float manaCost)
        {
            ManaCost = manaCost;
        }

        public void SpendMana(IMagicable magicable)
        {
            magicable.ModifyMana(-ManaCost);
        }

        public bool HasMana(IMagicable magicable)
        {
            return magicable.Mana >= ManaCost;
        }
    }
}