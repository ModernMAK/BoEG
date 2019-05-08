using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    [Serializable]
    public class HealthableMomento : IHealthable
    {
        public HealthableMomento()
        {
            _health = 0;
            _healthPercentage = 0;
            _healthCapacity = 0;
            _healthGeneration = 0;
        }

        public HealthableMomento(IHealthable healthable) : this()
        {
            Update(healthable);
        }

        
        public void Update(IHealthable healthable)
        {
            _health = healthable.Health;
            _healthPercentage = healthable.HealthPercentage;
            _healthCapacity = healthable.HealthCapacity;
            _healthGeneration = healthable.HealthGeneration;
        }

        [SerializeField] private float _health;
        [SerializeField] private float _healthPercentage;
        [SerializeField] private float _healthCapacity;
        [SerializeField] private float _healthGeneration;

        public float Health => _health;

        public float HealthPercentage => _healthGeneration;
        public float HealthCapacity => _healthCapacity;
        public float HealthGeneration => _healthGeneration;

        public void ModifyHealth(float change)
        {
            throw new InvalidOperationException("A momento cannot be altered.");
        }

        public void SetHealth(float health)
        {
            throw new InvalidOperationException("A momento cannot be altered.");
        }

        public event EventHandler<HealthableEventArgs> Modified;
        public event EventHandler<HealthableEventArgs> Modifying;
    }
}