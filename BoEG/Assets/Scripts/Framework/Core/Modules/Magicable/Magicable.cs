using System;
using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules
{
    //Im overcomplicating this for myself
    //Stop overthinking this. Do what unity does best


    public class Magicable : Statable, IInitializable<IMagicableData>, IMagicable, IListener<IStepableEvent>, IRespawnable, IListener<IModifiable>, IMagicableView
    {
        public Magicable(Actor actor) : base(actor)
        {
            _capacityModifiers = new ModifiedValueBoilerplate<IMagicCapacityModifier>(modifier=>modifier.MagicCapacity);
            _generationModifiers = new ModifiedValueBoilerplate<IMagicGenerationModifier>(modifier => modifier.MagicGeneration);
            _generationModifiers.ModifierRecalculated += OnModifierRecalculated;
            _capacityModifiers.ModifierRecalculated += OnModifierRecalculated;
        }

        private void OnModifierRecalculated(object sender, EventArgs e) => OnChanged();
        

        private readonly ModifiedValueBoilerplate<IMagicCapacityModifier> _capacityModifiers;
        private readonly ModifiedValueBoilerplate<IMagicGenerationModifier> _generationModifiers;
 
        public void Initialize(IMagicableData data)
        {
            StatCapacity.Base = data.MagicCapacity;
            Percentage = 1f;
            StatGeneration.Base = data.MagicGeneration;
        }

        public IMagicableView View => this;

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

        float IMagicableView.Capacity => Capacity.Total;

        float IMagicableView.Generation => Generation.Total;

        public IModifiedValue<float> Capacity => StatCapacity;

        public IModifiedValue<float> Generation => StatGeneration;

        protected override ModifiedValue StatCapacity => _capacityModifiers.Value;
        protected override ModifiedValue StatGeneration => _generationModifiers.Value;

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
            Percentage=1f;
        }
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