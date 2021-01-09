using System;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    //Im overcomplicating this for myself
    //Stop overthinking this. Do what unity does best


    [DisallowMultipleComponent]
    public class MagicableModule : StatableModule,
        IInitializable<IMagicableData>, IMagicable, IListener<IStepableEvent>, IRespawnable
    {
        public void Initialize(IMagicableData module)
        {
            _capacity = module.MagicCapacity;
            _percentage = 1f;
            _generation = module.MagicGeneration;
        }

        public float Magic
        {
            get => Stat;
            set => Stat = value;
        }

        public float MagicPercentage
        {
            get => StatPercentage;
            set => StatPercentage = value;
        }

        public float MagicCapacity
        {
            get => StatCapacity;
            set => StatCapacity = value;
        }

        public float MagicGeneration
        {
            get => StatGeneration;
            set => StatGeneration = value;
        }

        public event EventHandler<float> MagicChanged
        {
            add => StatChanged += value;
            remove => StatChanged -= value;
        }

        public bool HasMagic(float mana) => Magic > mana;
        public void SpendMagic(float mana) => Magic -= mana;


        protected override void OnModifierAdded(object sender, IModifier modifier)
        {
            if (modifier is IMagicableModifier magicableModifier)
            {
//                _modifiers.Add(magicableModifier);
            }
        }

        protected override void OnModifierRemoved(object sender, IModifier modifier)
        {
            if (modifier is IMagicableModifier magicableModifier)
            {
//                _modifiers.Remove(magicableModifier);
            }
        }

        private void OnPreStep(float deltaTime)
        {
            Generate(deltaTime);
        }

        public void Register(IStepableEvent source)
        {
            source.PreStep += OnPreStep;
        }

        public void Unregister(IStepableEvent source)
        {
            source.PreStep -= OnPreStep;
        }

        public void Respawn()
        {
            _percentage = 1f;
        }
    }


    public class Magicable : Statable,
        IInitializable<IMagicableData>, IMagicable, IListener<IStepableEvent>, IRespawnable
    {
        public Magicable(Actor actor) : base(actor)
        {
        }

        public void Initialize(IMagicableData module)
        {
            _capacity = module.MagicCapacity;
            _percentage = 1f;
            _generation = module.MagicGeneration;
        }

        public float Magic
        {
            get => Stat;
            set => Stat = value;
        }

        public float MagicPercentage
        {
            get => StatPercentage;
            set => StatPercentage = value;
        }

        public float MagicCapacity
        {
            get => StatCapacity;
            set => StatCapacity = value;
        }

        public float MagicGeneration
        {
            get => StatGeneration;
            set => StatGeneration = value;
        }

        public event EventHandler<float> MagicChanged
        {
            add => StatChanged += value;
            remove => StatChanged -= value;
        }

        public bool HasMagic(float mana) => Magic > mana;
        public void SpendMagic(float mana) => Magic -= mana;


        protected override void OnModifierAdded(object sender, IModifier modifier)
        {
            if (modifier is IMagicableModifier magicableModifier)
            {
//                _modifiers.Add(magicableModifier);
            }
        }

        protected override void OnModifierRemoved(object sender, IModifier modifier)
        {
            if (modifier is IMagicableModifier magicableModifier)
            {
//                _modifiers.Remove(magicableModifier);
            }
        }

        private void OnPreStep(float deltaTime)
        {
            Generate(deltaTime);
        }

        public void Register(IStepableEvent source)
        {
            source.PreStep += OnPreStep;
        }

        public void Unregister(IStepableEvent source)
        {
            source.PreStep -= OnPreStep;
        }

        public void Respawn()
        {
            _percentage = 1f;
        }
    }
}