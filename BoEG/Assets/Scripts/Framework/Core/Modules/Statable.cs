using System;
using MobaGame.Framework.Utility;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public class Statable : ActorModule, IListener<ILevelable>
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

        protected float StatPercentage
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

        protected float BaseStatCapacity
        {
            get => _capacity;
            set => _capacity = value;
        }

        protected float BaseStatGeneration
        {
            get => _generation;
            set => _generation = value;
        }
        protected virtual float StatCapacity => BaseStatCapacity;
        protected virtual float StatGeneration => BaseStatGeneration;


        public virtual void Register(ILevelable source)
        {
            source.LevelChanged += OnLevelChanged;
        }

        public void Unregister(ILevelable source)
        {
            source.LevelChanged -= OnLevelChanged;
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


        protected void Generate(float deltaTime)
        {
            Stat += StatGeneration * deltaTime;
        }

    }
  

}