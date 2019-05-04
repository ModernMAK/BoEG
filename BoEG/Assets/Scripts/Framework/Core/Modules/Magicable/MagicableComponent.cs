using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class MagicableComponent : MonoBehaviour, IComponent<IMagicable>, IMagicable
    {
        private IMagicable _magicable;
        public float Mana => _magicable.Mana;

        public float ManaPercentage => _magicable.ManaPercentage;

        public float ManaCapacity => _magicable.ManaCapacity;

        public float ManaGeneration => _magicable.ManaGeneration;

        public void ModifyMana(float change)
        {
            _magicable.ModifyMana(change);
        }

        public void SetMana(float mana)
        {
            _magicable.SetMana(mana);
        }

        public event EventHandler<MagicableEventArgs> Modified
        {
            add => _magicable.Modified += value;
            remove => _magicable.Modified -= value;
        }

        public event EventHandler<MagicableEventArgs> Modifying
        {
            add => _magicable.Modifying += value;
            remove => _magicable.Modifying -= value;
        }

        public void Initialize(IMagicable module)
        {
            _magicable = module;
        }
    }
}