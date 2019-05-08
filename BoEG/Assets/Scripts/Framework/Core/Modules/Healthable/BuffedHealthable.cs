using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class BuffedHealthable : IHealthable
    {
        public BuffedHealthable(IHealthable healthable, IBuffable buffable)
        {
            _healthable = healthable;
            BuffedHealthableUtil.CreateHealthGenerationBuff(buffable, out _capacityBonus, out _capacityMultiplier);
            BuffedHealthableUtil.CreateHealthCapacityBuff(buffable, out _generationBonus, out _generationMultiplier);
            healthable.Modified += ModifiedCatcher;
            healthable.Modifying += ModifyingCatcher;
        }

        private void ModifyingCatcher(object sender, HealthableEventArgs e)
        {
            OnModifying(e);
        }

        private void ModifiedCatcher(object sender, HealthableEventArgs e)
        {
            OnModified(e);
        }


        private readonly IValueBuffCalculator _capacityMultiplier;
        private readonly IValueBuffCalculator _capacityBonus;
        private readonly IValueBuffCalculator _generationMultiplier;
        private readonly IValueBuffCalculator _generationBonus;


        private readonly IHealthable _healthable;
        private float _normal;

        public float Health
        {
            get => HealthCapacity * HealthPercentage;
            protected set => HealthPercentage = value / HealthCapacity;
        }

        public float HealthPercentage
        {
            get => _normal;
            protected set => _normal = Mathf.Clamp01(value);
        }

        public float HealthCapacity => _healthable.HealthCapacity * _capacityMultiplier.Value + _capacityBonus.Value;

        public float HealthGeneration =>
            _healthable.HealthGeneration * _generationMultiplier.Value + _generationBonus.Value;


        public void ModifyHealth(float change)
        {
            var args = new HealthableEventArgs(change);
            OnModifying(args);
            Health += change;
            OnModified(args);
        }

        public void SetHealth(float health)
        {
            var change = health - Health;
            ModifyHealth(change);
        }

        public event EventHandler<HealthableEventArgs> Modified;
        public event EventHandler<HealthableEventArgs> Modifying;


        protected virtual void OnModified(HealthableEventArgs e)
        {
            Modified?.Invoke(this, e);
        }

        protected virtual void OnModifying(HealthableEventArgs e)
        {
            Modifying?.Invoke(this, e);
        }
    }
}