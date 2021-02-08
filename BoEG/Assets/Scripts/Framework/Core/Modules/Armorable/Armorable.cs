using System;
using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules
{
    public interface IPhysicalBlockModifier : IModifier
    {
        public FloatModifier PhysicalBlock { get; }
    }
    public interface IMagicalBlockModifier : IModifier
    {
        public FloatModifier MagicalBlock { get; }
    }
    public interface IPhysicalResistanceModifier : IModifier
    {
        public FloatModifier PhysicalResistance { get; }
    }
    public interface IMagicalResistanceModifier : IModifier
    {
        public FloatModifier MagicalResistance { get; }
    }
    public class Armorable : ActorModule, IArmorable, IInitializable<IArmorableData>, IListener<IModifiable>, IArmorableView
    {
        public Armorable(Actor actor) : base(actor)
        {
            Physical = new ModifiedArmor<IPhysicalBlockModifier, IPhysicalResistanceModifier>(block => block.PhysicalBlock, resist => resist.PhysicalResistance);
            Magical = new ModifiedArmor<IMagicalBlockModifier, IMagicalResistanceModifier>(block => block.MagicalBlock, resist => resist.MagicalResistance);
            Physical.Changed += OnSubViewChanged;
            Magical.Changed += OnSubViewChanged;
        }

        private void OnSubViewChanged(object sender, EventArgs e) => OnChanged();

        public IArmorableView View => this;

        public ModifiedArmor<IPhysicalBlockModifier,IPhysicalResistanceModifier> Physical { get; }
        IArmorView IArmorableView.Magical => Magical;

        IArmorView IArmorableView.Physical => Physical;

        public ModifiedArmor<IMagicalBlockModifier, IMagicalResistanceModifier> Magical { get; }

        IArmor IArmorable.Physical => Physical;

        IArmor IArmorable.Magical => Magical;

		public virtual Damage ResistDamage(Damage damage)
        {
            var reduction = CalculateReduction(damage);
            //Reduction, so negate the value
            var reducedDamage = damage.ModifyValue(-reduction, true);
            var resistingArgs = new ArmorableEventArgs(damage, reducedDamage);
            OnResisting(resistingArgs);
            var resistedArgs = new ArmorableEventArgs(resistingArgs.OutgoingDamage);
            OnResisted(resistedArgs);
            return resistedArgs.OutgoingDamage;
        }

        public float CalculateReduction(Damage damage)
        {
            var value = damage.Value;
            switch (damage.Type)
            {
                case DamageType.Physical:
                    return Physical.CalculateReduction(value);
                case DamageType.Magical:
                    return Magical.CalculateReduction(value);
                case DamageType.Pure:
                    return 0f;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public event EventHandler<ArmorableEventArgs> Resisted;
        public event EventHandler<ArmorableEventArgs> Resisting;

        public void Initialize(IArmorableData data)
        {
            Physical.Initialize(data.Physical);
            Magical.Initialize(data.Magical);
        }

        protected virtual void OnResisted(ArmorableEventArgs e)
        {
            Resisted?.Invoke(this, e);
        }

        protected virtual void OnResisting(ArmorableEventArgs e)
        {
            Resisting?.Invoke(this, e);
        }

		public void Register(IModifiable source)
		{
            Physical.Register(source);
            Magical.Register(source);
		}

		public void Unregister(IModifiable source)
        {
            Physical.Unregister(source);
            Magical.Unregister(source);
        }

        public event EventHandler Changed;
        protected void OnChanged() => Changed?.Invoke(this,EventArgs.Empty);
    }
}