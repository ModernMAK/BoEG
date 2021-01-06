using System;
using MobaGame.Framework.Types;
using MobaGame.Framework.Utility;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    //Im overcomplicating this for myself
    //Stop overthinking this. Do what unity does best


    // [DisallowMultipleComponent]
    public class Healthable : Statable,
        IInitializable<IHealthableData>, IHealthable, IListener<IStepableEvent>, IRespawnable
    {
        private event EventHandler<DeathEventArgs> _died;

        /// <summary>
        /// A flag set once died has been called.
        /// </summary>
        protected bool _isDead;

        
        public float Health
        {
            get => Stat;
            set => Stat = value;
        }

        public float HealthPercentage
        {
            get => StatPercentage;
            set => StatPercentage = value;
        }

        public float HealthCapacity
        {
            get => StatCapacity;
            set => StatCapacity = value;
        }

        public float HealthGeneration
        {
            get => StatGeneration;
            set => StatGeneration = value;
        }

        public event EventHandler<float> HealthChanged
        {
            add => StatChanged += value;
            remove => StatChanged -= value;
        }

        public event EventHandler<DeathEventArgs> Died
        {
            add => _died += value;
            remove => _died -= value;
        }


        public void Initialize(IHealthableData module)
        {
            StatCapacity = module.HealthCapacity;
            RawStatPercentage = 1f;
            StatGeneration = module.HealthGeneration;
        }


        protected override void OnModifierAdded(object sender, IModifier modifier)
        {
            if (modifier is IHealthableModifier healthableModifier)
            {
//                _modifiers.Add(healthableModifier);
            }
        }

        protected override void OnModifierRemoved(object sender, IModifier modifier)
        {
            if (modifier is IHealthableModifier healthableModifier)
            {
//                _modifiers.Remove(healthableModifier);
            }
        }

        protected virtual void OnPreStep(float deltaTime)
        {
            if (!HealthPercentage.SafeEquals(0f))
            {
                _isDead = false;
                Generate(deltaTime);
            }
        }

        protected virtual  void OnPostStep(float deltaTime)
        {
            if (!_isDead && HealthPercentage.SafeEquals(0f))
            {
                _isDead = true;
                OnDied(new DeathEventArgs(gameObject));
            }
        }

        public virtual void Register(IStepableEvent source)
        {
            source.PreStep += OnPreStep;
            source.PostStep += OnPostStep;
        }

        public virtual void Unregister(IStepableEvent source)
        {
            source.PreStep -= OnPreStep;
            source.PostStep -= OnPostStep;
        }

        protected virtual void OnDied(DeathEventArgs e)
        {
            _died?.Invoke(this, e);
        }

        public void Respawn()
        {
            _isDead = false;
            RawStatPercentage = 1f;
        }
    }
}