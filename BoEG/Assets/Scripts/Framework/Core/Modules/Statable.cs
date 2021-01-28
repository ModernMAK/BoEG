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
            _capacity = new ModifiedValue();
            _capacityGain = default;
            _generation = new ModifiedValue();
            _generationGain = default;
        }
        
        private ModifiedValue _capacity;
        private float _capacityGain;
        private ModifiedValue _generation;
        private float _generationGain;

        private float _percentage;

//        private List<IStatableModifier> _modifiers;

        protected virtual float Stat
        {
            get => StatPercentage * StatCapacity.Total;
            set => StatPercentage = value / StatCapacity.Total;
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

        protected ModifiedValue StatCapacity
        {
            get => _capacity;
        }

        protected ModifiedValue StatGeneration
        {
            get => _generation;
        }

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

        //DOES NOT RAISE STAT CHANGED
        protected void SetPercentage(float percentage) => _percentage = percentage;
            

        protected virtual void OnStatChanged(float e)
        {
            _statChanged?.Invoke(this, e);
        }

        protected virtual void OnLevelChanged(object sender, int levelDifference)
        {
            _capacity.Base += _capacityGain * levelDifference;
            _generation.Base += _generationGain * levelDifference;
        }


        protected void Generate(float deltaTime)
        {
            Stat += StatGeneration.Total * deltaTime;
        }

    }
  

}