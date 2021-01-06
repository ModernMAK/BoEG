using System;
using MobaGame.Framework.Utility;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    //Im overcomplicating this for myself
    //Stop overthinking this. Do what unity does best


    public class Statable : MonoBehaviour, IListener<ILevelable>, IListener<IModifiable>
    {
        private float _capacity;
        protected virtual float RawStatCapacity
        {
            get => _capacity;
            set => _capacity = value;
        }
        

        private float _capacityGain;
        protected virtual float RawStatCapacityGain
        {
            get => _capacityGain;
            set => _capacityGain = value;
        }
        private float _generation;
        protected virtual float RawStatGeneration
        {
            get => _generation;
            set => _generation = value;
        }
        private float _generationGain;
        protected virtual float RawStatGenerationGain
        {
            get => _generationGain;
            set => _generationGain = value;
        }
        
        private float _percentage;
        protected virtual float RawStatPercentage
        {
            get => _percentage;
            set => _percentage = value;
        }


//        private List<IStatableModifier> _modifiers;

        protected virtual float Stat
        {
            get => StatPercentage * StatCapacity;
            set => StatPercentage = value / StatCapacity;
        }

        protected virtual float StatPercentage
        {
            get => RawStatPercentage;
            set
            {
                value = Mathf.Clamp01(value);
                if (!RawStatPercentage.SafeEquals(value))
                    OnStatChanged(default);
                RawStatPercentage = value;
            }
        }

        protected virtual float StatCapacity
        {
            get => _capacity;
            set => _capacity = value;
        }

        protected virtual float StatGeneration
        {
            get => _generation;
            set => _generation = value;
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

        //TODO use these to add modifiers
//        protected ModifierResult _capacityModifier;
//        protected ModifierResult _generationModifier;
        protected event EventHandler<float> _statChanged;

        protected virtual event EventHandler<float> StatChanged
        {
            add => _statChanged += value;
            remove => _statChanged -= value;
        }

        protected virtual void OnStatChanged(float e)
        {
            _statChanged?.Invoke(this, e);
        }

        protected virtual void OnLevelChanged(object sender, int levelDifference)
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

        protected virtual void Generate(float deltaTime)
        {
            Stat += StatGeneration * deltaTime;
        }

    }
}