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

        public void Initialize(IMagicableData module)
        {
            StatCapacity.Base = module.MagicCapacity;
            SetPercentage(1f);
            StatGeneration.Base = module.MagicGeneration;
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

        public IModifiedValue<float> MagicCapacity => StatCapacity;

        public IModifiedValue<float> MagicGeneration => StatGeneration;

        public event EventHandler<float> MagicChanged
        {
            add => StatChanged += value;
            remove => StatChanged -= value;
        }

        public bool HasMagic(float mana) => Magic >= mana;
        public void SpendMagic(float mana) => Magic -= mana;


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