using System;
using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules
{
    //Im overcomplicating this for myself
    //Stop overthinking this. Do what unity does best


    public class Magicable : Statable, IInitializable<IMagicableData>, IMagicable, IListener<IStepableEvent>, IRespawnable
    {
        public Magicable(Actor actor) : base(actor)
        {
            _capacityModifiers = new MixedModifierList<IHealthCapacityModifier>();
            _generationModifiers = new MixedModifierList<IHealthGenerationModifier>();

            _capacityModifiers.ListChanged += RecalculateCapacityModifiers;
            _generationModifiers.ListChanged += RecalculateGenerationModifiers;
        }

        private MixedModifierList<IHealthCapacityModifier> _capacityModifiers;
        private MixedModifierList<IHealthGenerationModifier> _generationModifiers;
 
        private void RecalculateCapacityModifiers(object sender, EventArgs e)
        {
            StatCapacity.Modifier = _capacityModifiers.SumModifiers(mod => mod.HealthCapacity);
        }
        private void RecalculateGenerationModifiers(object sender, EventArgs e)
        {
            StatGeneration.Modifier = _generationModifiers.SumModifiers(mod => mod.HealthGeneration);
        }

        public void Initialize(IMagicableData data)
        {
            StatCapacity.Base = data.MagicCapacity;
            SetPercentage(1f);
            StatGeneration.Base = data.MagicGeneration;
        }

        public float Value
        {
            get => base.Stat;
            set => base.Stat = value;
        }

        public float Percentage
        {
            get => StatPercentage;
            set => StatPercentage = value;
        }

        public IModifiedValue<float> Capacity => StatCapacity;

        public IModifiedValue<float> Generation => StatGeneration;

        public event EventHandler<ChangedEventArgs<float>> ValueChanged
        {
            add => StatChanged += value;
            remove => StatChanged -= value;
        }

        public bool HasMagic(float mana) => Value >= mana;
        public void SpendMagic(float mana) => Value -= mana;


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
            SetPercentage(1f);
        }
    }
}