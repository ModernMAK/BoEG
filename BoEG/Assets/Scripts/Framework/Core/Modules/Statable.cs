using System;
using MobaGame.Framework.Utility;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public abstract class Statable : ActorModule, IListener<ILevelable>, IView
    {
        public Statable(Actor actor) : base(actor)
        {
            _percentage = default;
            _capacityGain = default;
            _generationGain = default;
        }

        private float _capacityGain;
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
                var changed = !_percentage.SafeEquals(value);
                if (changed)
                {
                    var cap = StatCapacity.Total;
                    var before = _percentage * cap;
                    var after = value * cap;
                    OnStatChanged(new ChangedEventArgs<float>(before, after));
                }
                _percentage = value;
                if (changed)
                    OnChanged();
            }
        }

        protected virtual void OnChanged() => Changed?.Invoke(this, EventArgs.Empty);

		protected abstract ModifiedValue StatCapacity
        {
            get;
        }

        protected abstract ModifiedValue StatGeneration
        {
            get;
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
        protected event EventHandler<ChangedEventArgs<float>> _statChanged;

        protected event EventHandler<ChangedEventArgs<float>> StatChanged
        {
            add => _statChanged += value;
            remove => _statChanged -= value;
        }

        public event EventHandler Changed;



        protected virtual void OnStatChanged(ChangedEventArgs<float> e)
        {
            _statChanged?.Invoke(this, e);
        }

        protected virtual void OnLevelChanged(object sender, ChangedEventArgs<int> e)
        {
            var levelDifference = e.After - e.Before;
            StatCapacity.Base += _capacityGain * levelDifference;
            StatGeneration.Base += _generationGain * levelDifference;
        }


        protected void Generate(float deltaTime)
        {
            Stat += StatGeneration.Total * deltaTime;
        }
    }
}