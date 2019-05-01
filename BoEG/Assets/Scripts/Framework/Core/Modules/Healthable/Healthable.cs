using System;
using Framework.Types;
using Framework.Utility;

namespace Framework.Core.Modules
{
    public class Healthable : Statable, IHealthable
    {
        public Healthable(float capacity, float generation) : base(capacity,generation)
        {
        }

        public virtual float Health
        {
            get => Value;
            protected set => Value = value;
        }

        public virtual float HealthPercentage
        {
            get => Normal;
            protected set => Normal = value;
        }

        public virtual float HealthCapacity
        {
            get => Capacity;
            protected set => Capacity = value;
        }

        public virtual float HealthGeneration
        {
            get => Generation;
            protected set => Generation = value;
        }

        public void ModifyHealth(float change)
        {
            var args = new HealthableEventArgs(change);
            OnModifying(args);
            Health += change;
            OnModified(args);
        }

        public void SetHealth(float health)
        {
            //Current + Change = Desired
            //Therefore
            //Desired - Current = Change
            float change = health - Health;
            ModifyHealth(change);
        }


        protected virtual void OnModified(HealthableEventArgs e)
        {
            Modified?.Invoke(this, e);
        }

        protected virtual void OnModifying(HealthableEventArgs e)
        {
            Modifying?.Invoke(this, e);
        }

        public event EventHandler<HealthableEventArgs> Modified;
        public event EventHandler<HealthableEventArgs> Modifying;
    }
}