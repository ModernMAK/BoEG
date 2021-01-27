using System;
using MobaGame.Framework.Types;
using MobaGame.Framework.Utility;
namespace MobaGame.Framework.Core.Modules
{
    
    public class Healthable : Statable, IInitializable<IHealthableData>, IHealthable, IListener<IStepableEvent>, IRespawnable, IListener<IModifiable>
    {
		public Healthable(Actor actor) : base(actor)
        {
            _capacityModifiers = new MixedModifierList<IHealthCapacityModifier>();
            _generationModifiers = new MixedModifierList<IHealthGenerationModifier>();

            _generationModifier = _capacityModifier = new Modifier();

            _capacityModifiers.ListChanged += RecalculateCapacityModifiers;
            _generationModifiers.ListChanged += RecalculateGenerationModifiers;
        }


        private void RecalculateCapacityModifiers(object sender, EventArgs e)
        {
            _capacityModifier = _capacityModifiers.SumModifiers(mod => mod.HealthCapacity);
        }
        private void RecalculateGenerationModifiers(object sender, EventArgs e)
        {
            _generationModifier = _generationModifiers.SumModifiers(mod => mod.HealthGeneration);
        }


        /// <summary>
        /// A flag set once died has been called.
        /// </summary>
        private bool _isDead;
        private MixedModifierList<IHealthCapacityModifier> _capacityModifiers;
        private MixedModifierList<IHealthGenerationModifier> _generationModifiers;
        private Modifier _capacityModifier;
        private Modifier _generationModifier;

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

        public float BaseHealthCapacity
        {
            get => BaseStatCapacity;
            set => BaseStatCapacity = value;
        }
        public float BonusHealthCapacity => _capacityModifier.Calculate(BaseHealthCapacity);
        public float HealthCapacity     => BaseHealthCapacity + BonusHealthCapacity;
        protected override float StatCapacity => HealthCapacity;

		public float BaseHealthGeneration
        {
            get => BaseStatGeneration;
            set => BaseStatGeneration = value;
        }
        public float BonusHealthGeneration => _generationModifier.Calculate(BaseHealthGeneration);
        public float HealthGeneration => BaseHealthGeneration + BonusHealthGeneration;
        protected override float StatGeneration => HealthGeneration;

        public event EventHandler<float> HealthChanged
        {
            add => StatChanged += value;
            remove => StatChanged -= value;
        }

        public event EventHandler<DeathEventArgs> Died
        {
            add => throw new Exception();
            remove => throw new Exception();
        }


        public void Initialize(IHealthableData module)
        {
            _capacity = module.HealthCapacity;
            _percentage = 1f;
            _generation = module.HealthGeneration;
        }


        private void OnPreStep(float deltaTime)
        {
            if (!HealthPercentage.SafeEquals(0f))
            {
                _isDead = false;
                Generate(deltaTime);
            }
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
            _isDead = false;
            _percentage = 1f;
        }

		public void Register(IModifiable source)
		{
            _capacityModifiers.Register(source);
		}

		public void Unregister(IModifiable source)
		{
            _capacityModifiers.Unregister(source);
		}
	}
}