using System;

namespace DeleteMe
{
    public class Armorable : IArmorable
    {
        public Armor Physical { get; private set; }
        public Armor Magical { get; private set; }

        public virtual Damage ResistDamage(Damage damage)
        {
            var reduction = CalculateReduction(damage);
            var args = new ArmorableEventArgs(damage,reduction);
            OnResisting(args);
            //Reduction, so negate the value
            var outDam = damage.ModifyValue(-reduction);
            OnResisted(args);
            return outDam;
        }

        public float CalculateReduction(Damage damage)
        {
            var value = damage.Value;
            switch (damage.Type)
            {
                case DamageType.Physical:
                    return CalculateReduction(value,Physical);
                case DamageType.Magical:
                    return CalculateReduction(value,Magical);
                case DamageType.Pure:
                    return 0f;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static float CalculateReduction(float damage, Armor armor)
        {
            if (armor.Immunity)
                return damage;
            else
            {
                //First apply block
                var result = armor.Resistance * (damage - armor.Block);
                var blocked = damage - result;
                //Only happens if Resistance > 1
                if (blocked > damage)
                    blocked = damage;
                return blocked;
            }
        }

/*
        protected Func<Damage, Damage> Callback { get; set; }
*/


        protected virtual void OnResisted(ArmorableEventArgs e)
        {
            Resisted?.Invoke(this, e);
        }

        protected virtual void OnResisting(ArmorableEventArgs e)
        {
            Resisting?.Invoke(this, e);
        }

        public event EventHandler<ArmorableEventArgs> Resisted;
        public event EventHandler<ArmorableEventArgs> Resisting;
    }

    public class ArmorableEventArgs : EventArgs
    {
        public ArmorableEventArgs(Damage damage, float reduction)
        {
            Damage = damage;
            Reduction = reduction;
        }

        public Damage Damage { get; private set; }
        public float Reduction { get; private set; }
    }

    public interface IArmorable
    {
        Armor Physical { get; }
        Armor Magical { get; }
        Damage ResistDamage(Damage damage);

        float CalculateReduction(Damage damage);
        
        event EventHandler<ArmorableEventArgs> Resisted;
        event EventHandler<ArmorableEventArgs> Resisting;
    }

    public struct Armor
    {
        public float Block { get; }
        public float Resistance { get; }
        public bool Immunity { get; }
    }
}