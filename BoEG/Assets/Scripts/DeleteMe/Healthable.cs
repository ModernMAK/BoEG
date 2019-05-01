using System;
using UnityEngine;

namespace DeleteMe
{
    public interface IHealthable
    {
        float Health { get; }
        float HealthPercentage { get; }
        float HealthCapacity { get; }
        float HealthGeneration { get; }
        void ModifyHealth(float change);
        void SetHealth(float health);
        event EventHandler<HealthableEventArgs> Modified;
        event EventHandler<HealthableEventArgs> Modifying;
    }

    public class Statable
    {
        protected Statable(float capacity, float generation)
        {
            _capacity = capacity;
            _normal = 1f;
            _generation = generation;
        }

        private float _normal;
        private float _capacity;
        private float _generation;

        protected float Value
        {
            get => Normal * Capacity;
            set => Normal = value / Capacity;
        }

        protected float Normal
        {
            get => _normal;
            set => _normal = Mathf.Clamp01(value);
        }

        protected float Capacity { 
            get => _capacity;
            set => _capacity = value; }

        protected float Generation
        {
            get => _generation;
            set => _generation = value;
        }
    }

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

    public class HealthableEventArgs : EventArgs
    {
        public HealthableEventArgs(float change)
        {
            Change = change;
        }

        public float Change { get; private set; }
    }

//    public interface IHealthable
//    {
//        float HealthPoints { get; }
//        float Normal { get; }
//        float HealthGeneration { get; }
//        float Capacity { get; }
//        bool IsDead { get; }
//
//
//        event HealthModifyHandler HealthPreModify;
//        event HealthModifyHandler HealthPostModify;
//        event HealthableHandler HealthPreDeath;
//        event HealthableHandler HealthPostDeath;
//
//        void Generate(float deltaTime);
//        void ModifyHealth(float deltaHealth);
//        void SetHealth(float health);
//    }
//
//    public delegate void HealthModifyHandler(object sender, HealthModifyEventArgs e);
//
//    public delegate void HealthableHandler(object sender, HealthableEventArgs e);
//
//    public class HealthModifyEventArgs : EventArgs
//    {
//        public HealthModifyEventArgs(float modified)
//        {
//            Modified = modified;
//        }
//
//        public float Modified { get; private set; }
//    }
//
//    public class HealthableEventArgs : EventArgs
//    {
//    }
//
//    public class HealthableTrait : IHealthable
//    {
//        public float HealthPoints
//        {
//            get { return Normal * HealthGeneration; }
//            set { Normal = HealthPoints / Capacity; }
//        }
//
//        private float _healthPercentage;
//
//        public float Normal
//        {
//            get { return _healthPercentage; }
//            private set { _healthPercentage = Mathf.Clamp01(value); }
//        }
//
//        public float HealthGeneration { get; private set; }
//        public float Capacity { get; private set; }
//        public bool IsDead { get; private set; }
//
//
//        public event HealthModifyHandler HealthPreModify;
//        public event HealthModifyHandler HealthPostModify;
//        public event HealthableHandler HealthPreDeath;
//        public event HealthableHandler HealthPostDeath;
//
//        public void ModifyHealth(float deltaHealth)
//        {
//            OnHealthPreModify(deltaHealth);
//            HealthPoints += deltaHealth;
//            OnHealthPostModify(deltaHealth);
//        }
//
//        public void SetHealth(float health)
//        {
//            var delta = health - HealthPoints;
//            ModifyHealth(delta);
//        }
//
//        public void Generate(float deltaTime)
//        {
//            Normal += deltaTime * HealthGeneration;
//        }
//
//        private void OnHealthPreModify(float delta)
//        {
//            HealthPreModify?.Invoke(this, new HealthModifyEventArgs(delta));
//        }
//
//        private void OnHealthPostModify(float delta)
//        {
//            HealthPostModify?.Invoke(this, new HealthModifyEventArgs(delta));
//        }
//
//        private void Die()
//        {
//            IsDead = true;
//        }
//    }
}