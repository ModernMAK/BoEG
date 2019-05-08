using System;
using Framework.Types;

namespace Framework.Core.Modules
{
    public class HealthableModule : IStepable, IHealthable
    {
        private readonly IHealthable _healthable;

        public HealthableModule(IHealthable healthable)
        {
            _healthable = healthable;
        }

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

//        protected override void Instantiate()
//        {
////            _data = GetData<IHealthableData>();
//        }

//        protected override void Spawn()
//        {
//            Health = Health.SetPercentage(1f);
//        }
        public void PreStep(float deltaTime)
        {
            ModifyHealth(HealthGeneration * deltaTime);
        }

        public void Step(float deltaTime)
        {
            //Do nothing
        }

        public void PostStep(float deltaTime)
        {
            //Do nothing
        }

        public void PhysicsStep(float deltaTime)
        {
            //Do nothing
        }

        public void Spawn()
        {
            SetHealth(HealthCapacity);
        }

        public void Despawn()
        {
            //Do Nothing
        }

        public void Instantiate()
        {
            SetHealth(HealthCapacity);
        }

        public void Terminate()
        {
            //Do Nothing
        }
    }
}