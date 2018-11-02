using Modules.Magicable;
using UnityEngine;

namespace Modules.Abilityable.Ability
{
    public abstract class AbilityManacost : Ability.AbilitySubmodule, IAbilityManacost
    {
        protected IMagicable Magicable { get; private set; }

        protected override void Initialize()
        {
            Magicable = Self.GetComponent<IMagicable>();
        }

        public abstract float ManaCost { get; }

        public bool RequiresMana
        {
            get { return ManaCost > 0; }
        }

        public bool HasEnoughMana
        {
            get { return Magicable.ManaPoints >= ManaCost; }
        }

        public void SpendMana()
        {
            Magicable.ModifyMana(-ManaCost, Self);
        }
        
        
        public class Flat : AbilityManacost
        {
            [SerializeField] private float _manaCost;

            public override float ManaCost
            {
                get { return _manaCost; }
            }
        }
        public class Scalar : AbilityManacost
        {
            [SerializeField] private float[] _manaCost;

            public override float ManaCost
            {
                get { return Ability.GetLeveledData(_manaCost); }
            }
        }
    }
}