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

            _generationModifier = _capacityModifier = new Modifier();

            _capacityModifiers.ListChanged += RecalculateCapacityModifiers;
            _generationModifiers.ListChanged += RecalculateGenerationModifiers;
        }

        private MixedModifierList<IHealthCapacityModifier> _capacityModifiers;
        private MixedModifierList<IHealthGenerationModifier> _generationModifiers;
        private Modifier _capacityModifier;
        private Modifier _generationModifier;

        private void RecalculateCapacityModifiers(object sender, EventArgs e)
        {
            _capacityModifier = _capacityModifiers.SumModifiers(mod => mod.HealthCapacity);
        }
        private void RecalculateGenerationModifiers(object sender, EventArgs e)
        {
            _generationModifier = _generationModifiers.SumModifiers(mod => mod.HealthGeneration);
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

        public float BaseMagicCapacity
        {
            get => BaseStatCapacity;
            set => BaseStatCapacity = value;
        }
        public float MagicCapacity => BaseMagicCapacity + BonusMagicCapacity;
        public float BonusMagicCapacity => _capacityModifier.Calculate(BaseMagicCapacity);
        protected override float StatCapacity => BaseMagicCapacity + BonusMagicCapacity;

		public float BaseMagicGeneration
        {
            get => BaseStatGeneration;
            set => BaseStatGeneration = value;
        }
        public float MagicGeneration => BaseMagicGeneration + BonusMagicGeneration;
        public float BonusMagicGeneration => _generationModifier.Calculate(BaseMagicGeneration);
        protected override float StatGeneration => MagicGeneration;

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
            _percentage = 1f;
        }
    }
}