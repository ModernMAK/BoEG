using System;
using System.ComponentModel;
using UnityEngine;

namespace Framework.Core.Modules
{
    /// <summary>
    /// Healthable component does not 
    /// </summary>
    public class HealthableComponent : MonoBehaviour, IComponent<IHealthable>, IHealthable
    {
        private IHealthable _healthable;

//        [SerializeField] private HealthableMomento _viewer;


        public void Initialize(IHealthable module)
        {
            _healthable = module;
        }
//
//        private void Awake()
//        {
//            _viewer = new HealthableMomento();
//        }
//
//        private void Update()
//        {
//            if (_healthable != null)
//                _viewer.Update(_healthable);
//        }

        public float Health => _healthable.Health;

        public float HealthPercentage => _healthable.HealthPercentage;

        public float HealthCapacity => _healthable.HealthCapacity;

        public float HealthGeneration => _healthable.HealthGeneration;

        public void ModifyHealth(float change)
        {
            _healthable.ModifyHealth(change);
        }

        public void SetHealth(float health)
        {
            _healthable.SetHealth(health);
        }

        public event EventHandler<HealthableEventArgs> Modified
        {
            add => _healthable.Modified += value;
            remove => _healthable.Modified -= value;
        }

        public event EventHandler<HealthableEventArgs> Modifying
        {
            add => _healthable.Modifying += value;
            remove => _healthable.Modifying -= value;
        }
    }
}