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


    public class Statable : MonoBehaviour,
        IStepable, IListener<ILevelable>, IListener<IModifiable>
    {
        protected float _percentage;
        protected float _capacity;
        protected float _generation;


        protected float _capacityGain;
        protected float _generationGain;

        //TODO use these to add modifiers
//        protected ModifierResult _capacityModifier;
//        protected ModifierResult _generationModifier;
        protected event EventHandler<float> _statChanged;

//        private List<IStatableModifier> _modifiers;

        protected virtual float Stat
        {
            get => StatPercentage * StatCapacity;
            set => StatPercentage = value / StatCapacity;
        }

        protected virtual  float StatPercentage
        {
            get => _percentage;
            set
            {
                value = Mathf.Clamp01(value);
                if (!_percentage.SafeEquals(value))
                    OnStatChanged(default);
                _percentage = value;
            }
        }

        protected virtual  float StatCapacity
        {
            get => _capacity;
            set => _capacity = value;
        }

        protected virtual  float StatGeneration
        {
            get => _generation;
            set => _generation = value;
        }

        protected virtual event EventHandler<float> StatChanged
        {
            add => _statChanged += value;
            remove => _statChanged -= value;
        }


        public virtual void Register(ILevelable source)
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

        protected virtual void OnStatChanged(float e)
        {
            _statChanged?.Invoke(this, e);
        }

        protected virtual  void OnLevelChanged(object sender, int levelDifference)
        {
            _capacity += _capacityGain * levelDifference;
            _generation += _generationGain * levelDifference;
        }

        protected virtual void OnModifierAdded(object sender, IModifier modifier)
        {

        }

        protected virtual void OnModifierRemoved(object sender, IModifier modifier)
        {

        }

        protected virtual void Generate() => Stat += StatGeneration;
        public virtual void PreStep(float deltaTime)
        {
        }

        public virtual void Step(float deltaTime)
        {
        }

        public virtual void PostStep(float deltaTime)
        {
        }

        public virtual void PhysicsStep(float deltaTime)
        {
        }
    }
}