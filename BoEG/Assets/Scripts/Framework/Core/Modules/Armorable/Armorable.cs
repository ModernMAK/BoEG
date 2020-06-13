using System;
using Framework.Types;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class Armorable : MonoBehaviour, IArmorable, IInitializable<IArmorableData>
    {
        public void Initialize(IArmorableData data)
        {
            Physical = new Armor(data.Physical);
            Magical = new Armor(data.Magical);
        }


        public Armor Physical { get; private set; }
        public Armor Magical { get; private set; }

        public virtual Damage ResistDamage(Damage damage)
        {
            var reduction = CalculateReduction(damage);
            var args = new ArmorableEventArgs() {Damage = damage, Reduction = reduction};
            OnResisting(args);
            //Reduction, so negate the value
            args.Damage = args.Damage.ModifyValue(-args.Reduction);
            OnResisted(args);
            return args.Damage;
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
}