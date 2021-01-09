using System;
using MobaGame.Framework.Types;
using MobaGame.Framework.Utility;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    //Im overcomplicating this for myself
    //Stop overthinking this. Do what unity does best

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Actor))]
    public class HealthableModule : MonoBehaviour,
        IInitializable<IHealthableData>, IHealthable, IListener<IStepableEvent>, IRespawnable
    {
        private Healthable _healthable;

        private void Awake()
        {
            _healthable = new Healthable(GetComponent<Actor>());
        }

        public void Initialize(IHealthableData module) => _healthable.Initialize(module);

        public float Health
        {
            get => _healthable.Health;
            set => _healthable.Health = value;
        }

        public float HealthPercentage
        {
            get => _healthable.HealthPercentage;
            set => _healthable.HealthPercentage = value;
        }

        public float HealthCapacity 
        {
            get => _healthable.HealthCapacity;
            set => _healthable.HealthCapacity = value;
        }
        public float HealthGeneration 
        {
            get => _healthable.HealthGeneration;
            set => _healthable.HealthGeneration = value;
        }
        public event EventHandler<float> HealthChanged
        {
            add => _healthable.HealthChanged += value;
            remove => _healthable.HealthChanged -= value;
        }
        public event EventHandler<DeathEventArgs> Died
        {
            add => _healthable.Died += value;
            remove => _healthable.Died -= value;
        }

        public void Register(IStepableEvent source) => _healthable.Register(source);

        public void Unregister(IStepableEvent source) => _healthable.Unregister(source);

        public void Respawn() => _healthable.Respawn();
    }
}