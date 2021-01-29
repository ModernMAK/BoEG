using System;
using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules
{

    /// <summary>
    /// A Self Contained Modifier List & Modified Value
    /// 
    /// </summary>
    /// <typeparam name="TModifier"></typeparam>
    public class ModifiedValueBoilerplate<TModifier> : IListener<IModifiable> where TModifier : IModifier
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <remarks>List defaults to a Mixed Modifier List.</remarks>
        public ModifiedValueBoilerplate(Func<TModifier, Modifier> func) : this(new MixedModifierList<TModifier>(), func) { }
        public ModifiedValueBoilerplate(ModifierList<TModifier> list, Func<TModifier,Modifier> func) 
		{
            Value = new ModifiedValue();
            List = list;
            GetModifier = func;
            List.ListChanged += RecalculateModifier;
            
		}
        private Func<TModifier, Modifier> GetModifier { get; }
        public ModifiedValue Value { get; }
        public ModifierList<TModifier> List { get; }

		void RecalculateModifier(object sender, EventArgs e) => Value.Modifier = List.SumModifiers(GetModifier);

		public void Register(IModifiable source) => List.Register(source);
		

		public void Unregister(IModifiable source) => List.Unregister(source);
    }

    public class Healthable : Statable, IInitializable<IHealthableData>, IHealthable, IListener<IStepableEvent>,
        IRespawnable, IListener<IModifiable>
    {
        #region Constructors
        
        public Healthable(Actor actor) : base(actor)
        {
            _capacityModifiers = new ModifiedValueBoilerplate<IHealthCapacityModifier>(modifier=>modifier.HealthCapacity);
            _generationModifiers = new ModifiedValueBoilerplate<IHealthGenerationModifier>(modifier=>modifier.HealthGeneration);

        }
        
        #endregion

        #region Events

        public event EventHandler<ChangedEventArgs<float>> ValueChanged
        {
            add => StatChanged += value;
            remove => StatChanged -= value;
        }

        #endregion

        #region Variables

        private readonly ModifiedValueBoilerplate<IHealthCapacityModifier> _capacityModifiers;
        private readonly ModifiedValueBoilerplate<IHealthGenerationModifier> _generationModifiers;

        #endregion

        #region Properties

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

        protected override ModifiedValue StatCapacity => _capacityModifiers.Value;

        protected override ModifiedValue StatGeneration => _generationModifiers.Value;

		#endregion

		#region IInitializable<IHealthableData>

		public void Initialize(IHealthableData data)
        {
            StatCapacity.Base = data.Capacity;
            SetPercentage(1f);
            StatGeneration.Base = data.Generation;
        }

        #endregion

        #region IRespawnable

        public void Respawn() => SetPercentage(1f);

        #endregion

        #region IListenable<ISteppableEvent>

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

        #endregion
        
        #region IListener<IModifiable>

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


        #endregion
    }
}