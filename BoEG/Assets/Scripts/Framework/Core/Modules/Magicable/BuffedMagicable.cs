using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class BuffedMagicable : IMagicable
    {
        public BuffedMagicable(IMagicable magicable, IBuffable buffable)
        {
            _magicable = magicable;
            BuffedMagicableUtil.CreateManaGenerationBuff(buffable, out _capacityBonus, out _capacityMultiplier);
            BuffedMagicableUtil.CreateManaCapacityBuff(buffable, out _generationBonus, out _generationMultiplier);
            magicable.Modified += ModifiedCatcher;
            magicable.Modifying += ModifyingCatcher;
        }

        private void ModifyingCatcher(object sender, MagicableEventArgs e)
        {
            OnModifying(e);
        }

        private void ModifiedCatcher(object sender, MagicableEventArgs e)
        {
            OnModified(e);
        }


        private readonly IValueBuffCalculator _capacityMultiplier;
        private readonly IValueBuffCalculator _capacityBonus;
        private readonly IValueBuffCalculator _generationMultiplier;
        private readonly IValueBuffCalculator _generationBonus;


        private readonly IMagicable _magicable;
        private float _normal;

        public float Mana
        {
            get => ManaCapacity * ManaPercentage;
            protected set => ManaPercentage = value / ManaCapacity;
        }

        public float ManaPercentage
        {
            get => _normal;
            protected set => _normal = Mathf.Clamp01(value);
        }

        public float ManaCapacity => _magicable.ManaCapacity * _capacityMultiplier.Value + _capacityBonus.Value;

        public float ManaGeneration =>
            _magicable.ManaGeneration * _generationMultiplier.Value + _generationBonus.Value;


        public void ModifyMana(float change)
        {
            var args = new MagicableEventArgs(change);
            OnModifying(args);
            Mana += change;
            OnModified(args);
        }

        public void SetMana(float mana)
        {
            var change = mana - Mana;
            ModifyMana(change);
        }

        public event EventHandler<MagicableEventArgs> Modified;
        public event EventHandler<MagicableEventArgs> Modifying;


        protected virtual void OnModified(MagicableEventArgs e)
        {
            Modified?.Invoke(this, e);
        }

        protected virtual void OnModifying(MagicableEventArgs e)
        {
            Modifying?.Invoke(this, e);
        }
    }
}