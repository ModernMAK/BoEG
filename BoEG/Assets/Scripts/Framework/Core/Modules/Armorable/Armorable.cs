using System;
using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules
{
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

		public virtual SourcedDamage ResistDamage(SourcedDamage damage)
        {
            OnResisting(damage);
            damage = CalculateDamageMitigation(damage);
            var reduction = CalculateReduction(damage);
            //Reduction, so negate the value
            var reducedDamage = damage.ModifyDamageValue(-reduction, true);
            damage = reducedDamage;
            var resisted = new ChangedEventArgs<SourcedDamage>(damage, reducedDamage);
            OnResisted(resisted);
            return damage;
        }

        public float CalculateReduction(SourcedDamage damage)
        {
            var value = damage.Value.Value;
            switch (damage.Value.Type)
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

        public event EventHandler<ChangedEventArgs<SourcedDamage>> Resisted;
        public event EventHandler<SourcedDamage> Resisting;
        public event EventHandler<ChangableEventArgs<SourcedDamage>> DamageMitigation;

        public void Initialize(IArmorableData data)
        {
            Physical.Initialize(data.Physical);
            Magical.Initialize(data.Magical);
        }

        protected virtual void OnResisted(ChangedEventArgs<SourcedDamage> e)
        {
            Resisted?.Invoke(this, e);
        }

        protected virtual void OnResisting(SourcedDamage e)
        {
            Resisting?.Invoke(this, e);
        }

        protected virtual SourcedDamage CalculateDamageMitigation(SourcedDamage e) =>
            DamageMitigation.CalculateChange(this, e);

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