using System;
using System.Collections.Generic;
using Framework.Types;
using Framework.Utility;
using Old.Modules.Levelable;
using UnityEngine;

namespace Framework.Core.Modules
{
    //Im overcomplicating this for myself
    //Stop overthinking this. Do what unity does best


    [DisallowMultipleComponent]
    public class Healthable : MonoBehaviour,
        IComponent<IHealthableData>, IHealthable, IStepable,
        IListener<ILevelable>, IListener<IModifiable>
    {
        private float _percentage;
        private float _capacity;
        private float _generation;


        private float _capacityGain;
        private float _generationGain;

        //TODO use these to add modifiers
        private ModifierResult _capacityModifier;
        private ModifierResult _generationModifier;
        private event EventHandler<float> _healthChanged;

        private List<IHealthableModifier> _modifiers;

        public float Health
        {
            get => HealthPercentage * HealthCapacity;
            set => HealthPercentage = value / HealthCapacity;
        }

        public float HealthPercentage
        {
            get => _percentage;
            set
            {
                value = Mathf.Clamp01(value);
                if (!_percentage.SafeEquals(value))
                    OnHealthChanged(default);
                _percentage = value;
            }
        }

        public float HealthCapacity
        {
            get => _capacity;
            set => _capacity = value;
        }

        public float HealthGeneration
        {
            get => _generation;
            set => _generation = value;
        }

        public event EventHandler<float> HealthChanged
        {
            add => _healthChanged += value;
            remove => _healthChanged -= value;
        }


        public void Initialize(IHealthableData module)
        {
            _capacity = module.HealthCapacity;
            _percentage = 1f;
            _generation = module.HealthGeneration;
        }

        public void Register(ILevelable source)
        {
            source.LevelChanged += OnLevelChanged;
        }

        public void Unregister(ILevelable source)
        {
            source.LevelChanged -= OnLevelChanged;
        }

        public void Register(IModifiable source)
        {
            source.OnModifierAdded += OnModifierAdded;
            source.OnModifierRemoved += OnModifierRemoved;
        }

        public void Unregister(IModifiable source)
        {
            source.OnModifierAdded -= OnModifierAdded;
            source.OnModifierRemoved -= OnModifierRemoved;
        }

        protected virtual void OnHealthChanged(float e)
        {
            _healthChanged?.Invoke(this, e);
        }

        private void OnLevelChanged(object sender, int levelDifference)
        {
            _capacity += _capacityGain * levelDifference;
            _generation += _generationGain * levelDifference;
        }

        private void OnModifierAdded(object sender, IModifier modifier)
        {
            if (modifier is IHealthableModifier healthableModifier)
            {
                _modifiers.Add(healthableModifier);
            }
        }

        private void OnModifierRemoved(object sender, IModifier modifier)
        {
            if (modifier is IHealthableModifier healthableModifier)
            {
                _modifiers.Remove(healthableModifier);
            }
        }

        public void PreStep(float deltaTime)
        {
            if (!HealthPercentage.SafeEquals(0f))
                Health += HealthGeneration;
        }

        public void Step(float deltaTime)
        {
            throw new NotImplementedException();
        }

        public void PostStep(float deltaTime)
        {
            throw new NotImplementedException();
        }

        public void PhysicsStep(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}