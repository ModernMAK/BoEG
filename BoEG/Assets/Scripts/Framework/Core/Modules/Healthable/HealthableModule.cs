using System;

namespace Framework.Core.Modules
{
    public class HealthableModule : IModule, IHealthable
    {
//        protected override void PreStep(float delta)
//        {
//            if (IsSpawned)
//                GenerateHealth(delta);
//        }
//
//        protected override void PostStep(float deltaStep)
//        {
//            base.PostStep(deltaStep);
//            _momento = new HealthableMomento(this);
//        }

//        [SerializeField] private HealthableMomento _momento;
//
//        private void GenerateHealth(float delta)
//        {
//            if (!IsDead)
//            {
//                ModifyPoints(Health.Generation * delta);
//            }
//        }
//
//        private void ModifyPoints(float deltaPoints)
//        {
//            var nPoints = Mathf.Clamp(Health.Points + deltaPoints, 0f, Health.Capacity);
//            Health = Health.SetPoints(nPoints);
//        }

        /// <summary>
        /// The Bit Mask used for serialization.
        /// </summary>
//        private byte _dirtyMask;

        private readonly IHealthable _healthable;

        public HealthableModule(IHealthable healthable)
        {
            _healthable = healthable;
        }

//        private IHealthableData _data;

//        public PointData Health { get; private set; }

//        public bool IsDead
//        {
//            get { return Health.Percentage <= 0f; }
//        }

        public float Health => _healthable.Health;

        public float HealthPercentage => _healthable.HealthPercentage;

        public float HealthCapacity => _healthable.HealthCapacity;

        public float HealthGeneration => _healthable.HealthGeneration;

//        public void ModifyHealth(float deltaValue)
//        {
//            if (!IsSpawned)
//                return;
//
//            var wasDead = IsDead;
//            OnHealthModifying(deltaValue);
//            ModifyPoints(deltaValue);
//            OnHealthModified(deltaValue);
//            if (!wasDead && IsDead)
//                OnDied();
//        }

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

//        public event HealthChangeHandler HealthModified;
//        public event HealthChangeHandler HealthModifying;
//        public event DeathHandler Died;

//        protected void OnDied()
//        {
//            if (Died != null)
//                Died();
//        }
//
//        protected void OnHealthModified(float deltaValue)
//        {
//            if (HealthModified != null)
//                HealthModified(deltaValue);
//        }
//
//        protected void OnHealthModifying(float deltaValue)
//        {
//            if (HealthModifying != null)
//                HealthModifying(deltaValue);
//        }

//        protected override void Instantiate()
//        {
////            _data = GetData<IHealthableData>();
//        }

//        protected override void Spawn()
//        {
//            Health = Health.SetPercentage(1f);
//        }
        public void PreStep(float delta)
        {
            ModifyHealth(HealthGeneration * delta);
        }

        public void Step(float delta)
        {
            //Do nothing
        }

        public void PostStep(float delta)
        {
            //Do nothing
        }

        public void PhysicsStep(float delta)
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