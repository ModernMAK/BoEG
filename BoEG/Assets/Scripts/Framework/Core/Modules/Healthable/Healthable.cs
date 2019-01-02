using Framework.Types;
using Framework.Utility;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class Healthable : Module, IHealthable
    {
        protected override void PreStep(float delta)
        {
            if (IsSpawned)
                GenerateHealth(delta);
        }

        private void GenerateHealth(float delta)
        {
            if (!IsDead)
            {
                ModifyPoints(Health.Generation * delta);
            }
        }

        private void ModifyPoints(float deltaPoints)
        {
            var nPoints = Mathf.Clamp(Health.Points + deltaPoints, 0f, Health.Capacity);
            Health = Health.SetPoints(nPoints);
        }

        /// <summary>
        /// The Bit Mask used for serialization.
        /// </summary>
        private byte _dirtyMask;

        private IHealthableData _data;

        public PointData Health { get; private set; }

        public bool IsDead
        {
            get { return Health.Percentage <= 0f; }
        }

        public void ModifyHealth(float deltaValue)
        {
            if (!IsSpawned)
                return;

            var wasDead = IsDead;
            OnHealthModifying(deltaValue);
            ModifyPoints(deltaValue);
            OnHealthModified(deltaValue);
            if (!wasDead && IsDead)
                OnDied();
        }

        public event HealthChangeHandler HealthModified;
        public event HealthChangeHandler HealthModifying;
        public event DeathHandler Died;

        protected void OnDied()
        {
            if (Died != null)
                Died();
        }

        protected void OnHealthModified(float deltaValue)
        {
            if (HealthModified != null)
                HealthModified(deltaValue);
        }

        protected void OnHealthModifying(float deltaValue)
        {
            if (HealthModifying != null)
                HealthModifying(deltaValue);
        }

        protected override void Instantiate()
        {
            _data = GetData<IHealthableData>();
        }

        protected override void Spawn()
        {
            Health = Health.SetPercentage(1f);
        }
    }
}