using System;
using MobaGame.Framework.Types;
using MobaGame.Framework.Utility;
namespace MobaGame.Framework.Core.Modules
{
    
    public class Healthable : Statable, IInitializable<IHealthableData>, IHealthable, IListener<IStepableEvent>, IRespawnable, IListener<IModifiable>
    {
		private struct ModifierResult : IHealthableModifier
		{
            public ModifierResult(float capBonus, float capFlatMul, float capMultiMul, float genBonus, float genFlatMul, float genMultiMul)
			{
                HealthCapacityBonus = capBonus;
                HealthCapacityFlatMultiplier = capFlatMul;
                HealthCapacityMultiplicativeMultiplier = capMultiMul;

                HealthGenerationBonus = genBonus;
                HealthGenerationMultiplicativeMultiplier = genMultiMul;
                HealthGenerationFlatMultiplier = genFlatMul;

            }
			public float HealthCapacityBonus { get; }

			public float HealthCapacityFlatMultiplier { get; }

			public float HealthCapacityMultiplicativeMultiplier { get; }

            public float HealthGenerationBonus { get; }

            public float HealthGenerationFlatMultiplier { get; }

            public float HealthGenerationMultiplicativeMultiplier { get; }

            public ModifierResult AddModifier(IHealthableModifier modifier)
            {
                var hcb = HealthCapacityBonus + modifier.HealthCapacityBonus;
                var hcmm = HealthCapacityMultiplicativeMultiplier + modifier.HealthCapacityMultiplicativeMultiplier;
                var hcfm = HealthCapacityFlatMultiplier + modifier.HealthCapacityFlatMultiplier;

                var hgb = HealthGenerationBonus + modifier.HealthGenerationBonus;
                var hgmm = HealthGenerationMultiplicativeMultiplier + modifier.HealthGenerationMultiplicativeMultiplier;
                var hgfm = HealthGenerationFlatMultiplier + modifier.HealthGenerationFlatMultiplier;
                return new ModifierResult(hcb, hcfm, hcmm, hgb, hgfm, hgmm);

            }
        }
		public Healthable(Actor actor) : base(actor)
        {
            _isDead = default;
            _modifiers = new MixedModifierList<IHealthableModifier>();
            _modifiers.ListChanged += RecaulculateModifiers;
        }


		private void RecaulculateModifiers(object sender, EventArgs e)
		{
            var result = new ModifierResult();
            foreach(var m in _modifiers)
			{
                result.AddModifier(m);
			}
            _cachedResult = result;
		}

		private event EventHandler<DeathEventArgs> _died;

        /// <summary>
        /// A flag set once died has been called.
        /// </summary>
        private bool _isDead;
		private MixedModifierList<IHealthableModifier> _modifiers;
        private ModifierResult _cachedResult;

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
            get => StatCapacity;
            set => StatCapacity = value;
        }
        public float BonusHealthCapacity => _cachedResult.HealthCapacityBonus + BaseHealthGeneration * (_cachedResult.HealthCapacityFlatMultiplier + _cachedResult.HealthCapacityMultiplicativeMultiplier);
        public float HealthCapacity     => BaseHealthCapacity + BonusHealthCapacity;
        

        public float BaseHealthGeneration
        {
            get => StatGeneration;
            set => StatGeneration = value;
        }
        public float BonusHealthGeneration => _cachedResult.HealthGenerationBonus + BaseHealthGeneration * (_cachedResult.HealthGenerationFlatMultiplier + _cachedResult.HealthGenerationMultiplicativeMultiplier);
        public float HealthGeneration => BaseHealthGeneration + BonusHealthGeneration;
        
        public event EventHandler<float> HealthChanged
        {
            add => StatChanged += value;
            remove => StatChanged -= value;
        }

        public event EventHandler<DeathEventArgs> Died
        {
            add => _died += value;
            remove => _died -= value;
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

        private void OnPostStep(float deltaTime)
        {
            if (!_isDead && HealthPercentage.SafeEquals(0f))
            {
                _isDead = true;
                OnDied(new DeathEventArgs(GameObject));
            }
        }

        public void Register(IStepableEvent source)
        {
            source.PreStep += OnPreStep;
            source.PostStep += OnPostStep;
        }

        public void Unregister(IStepableEvent source)
        {
            source.PreStep -= OnPreStep;
            source.PostStep -= OnPostStep;
        }

        protected virtual void OnDied(DeathEventArgs e)
        {
            _died?.Invoke(this, e);
        }

        public void Respawn()
        {
            _isDead = false;
            _percentage = 1f;
        }

		public void Register(IModifiable source)
		{
            _modifiers.Register(source);
		}

		public void Unregister(IModifiable source)
		{
            _modifiers.Unregister(source);
		}
	}
}