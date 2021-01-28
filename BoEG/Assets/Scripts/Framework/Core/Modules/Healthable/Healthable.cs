using System;
using MobaGame.Framework.Types;
namespace MobaGame.Framework.Core.Modules
{
    
    public class Healthable : Statable, IInitializable<IHealthableData>, IHealthable, IListener<IStepableEvent>, IRespawnable, IListener<IModifiable>
    {
		public Healthable(Actor actor) : base(actor)
        {
            _capacityModifiers = new MixedModifierList<IHealthCapacityModifier>();
            _generationModifiers = new MixedModifierList<IHealthGenerationModifier>();

            _capacityModifiers.ListChanged += RecalculateCapacityModifiers;
            _generationModifiers.ListChanged += RecalculateGenerationModifiers;
        }


        private void RecalculateCapacityModifiers(object sender, EventArgs e)
        {
            StatCapacity.Modifier = _capacityModifiers.SumModifiers(mod => mod.HealthCapacity);
        }
        private void RecalculateGenerationModifiers(object sender, EventArgs e)
        {
            StatGeneration.Modifier = _generationModifiers.SumModifiers(mod => mod.HealthGeneration);
        }


        /// <summary>
        /// A flag set once died has been called.
        /// </summary>
        private MixedModifierList<IHealthCapacityModifier> _capacityModifiers;
        private MixedModifierList<IHealthGenerationModifier> _generationModifiers;

        public float Value
        {
            get => Stat;
            set => Stat = value;
        }

        public float Percentage
        {
            get => StatPercentage;
            set => StatPercentage = value;
        }

        public IModifiedValue<float> Capacity => StatCapacity;
        public IModifiedValue<float> Generation => StatGeneration;


		public event EventHandler<float> ValueChanged
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
            StatCapacity.Base = module.Capacity;
            SetPercentage(1f);
            StatGeneration.Base = module.Generation;
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


        public void Respawn() => SetPercentage(1f);

		public void Register(IModifiable source)
		{
            _capacityModifiers.Register(source);
            _generationModifiers.Register(source);
		}

		public void Unregister(IModifiable source)
		{
            _capacityModifiers.Unregister(source);
            _generationModifiers.Unregister(source);
		}
	}
}