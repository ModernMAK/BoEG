using System;
using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules
{
    public class Armorable : ActorModule, IArmorable, IInitializable<IArmorableData>
    {
        public Armorable(Actor actor) : base(actor)
        {
            Physical = default;
            Magical = default;
        }
        
        public Armor Physical { get; private set; }
        public Armor Magical { get; private set; }

        public virtual Damage ResistDamage(Damage damage)
        {
            var reduction = CalculateReduction(damage);
            var reducedDamage = damage.ModifyValue(-reduction, true);
            var resistingArgs = new ArmorableEventArgs(damage, reducedDamage);
            OnResisting(resistingArgs);
            var resistedArgs = new ArmorableEventArgs(resistingArgs.OutgoingDamage);
            //Reduction, so negate the value
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
            Physical = new Armor(data.Physical);
            Magical = new Armor(data.Magical);
        }

        protected virtual void OnResisted(ArmorableEventArgs e)
        {
            Resisted?.Invoke(this, e);
        }

        protected virtual void OnResisting(ArmorableEventArgs e)
        {
            Resisting?.Invoke(this, e);
        }
    }
}