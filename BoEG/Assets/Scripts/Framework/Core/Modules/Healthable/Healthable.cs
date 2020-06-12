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
    public class Healthable : Statable,
        IInitializable<IHealthableData>, IHealthable
    {
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


        public void Initialize(IHealthableData module)
        {
            _capacity = module.HealthCapacity;
            _percentage = 1f;
            _generation = module.HealthGeneration;
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

        public override void PreStep(float deltaTime)
        {
            if (!HealthPercentage.SafeEquals(0f))
                Generate();
        }
    }
}