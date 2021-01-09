using System;
using MobaGame.Framework.Utility;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public class Statable : ActorModule, IListener<ILevelable>, IListener<IModifiable>
    {
        public Statable(Actor actor) : base(actor)
        {
            _percentage = default;
            _capacity = default;
            _capacityGain = default;
            _generation = default;
            _generationGain = default;
        }
        
        protected float _capacity;


        protected float _capacityGain;
        protected float _generation;
        protected float _generationGain;

        protected float _percentage;

//        private List<IStatableModifier> _modifiers;

        protected virtual float Stat
        {
            get => StatPercentage * StatCapacity;
            set => StatPercentage = value / StatCapacity;
        }

        protected virtual float StatPercentage
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
    //Im overcomplicating this for myself
    //Stop overthinking this. Do what unity does best


    public class StatableModule : MonoBehaviour, IListener<ILevelable>, IListener<IModifiable>
    {
        protected float _capacity;


        protected float _capacityGain;
        protected float _generation;
        protected float _generationGain;

        protected float _percentage;

//        private List<IStatableModifier> _modifiers;

        protected virtual float Stat
        {
            get => StatPercentage * StatCapacity;
            set => StatPercentage = value / StatCapacity;
        }

        protected virtual float StatPercentage
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