using System;
using Core;
using Util;

namespace Modules.Armorable
{
    [Serializable]
    public class Armorable : IArmorable
    {
        public Armorable(IArmorableData data)
        {
            _data = data;
        }

        private readonly IArmorableData _data;

        public Armor Physical
        {
            get { return _data.Physical.Evaluate(); }
        }

        public Armor Magical
        {
            get { return _data.Magical.Evaluate(); }
        }


        public Damage CalculateDamageAfterReductions(Damage damage)
        {
            var value = damage.Value;
            switch (damage.Type)
            {
                case DamageType.Physical:
                    value = Physical.CalculateReduction(value);
                    break;
                case DamageType.Magical:
                    value = Magical.CalculateReduction(value);
                    break;
                case DamageType.Pure:
                case DamageType.Modification:
                    break;
                default:
                    break;
            }

            return new Damage(value, damage.Type, damage.Source);
        }

        public Damage ResistDamage(Damage damage)
        {
            var newDamage = CalculateDamageAfterReductions(damage);
            OnResisted();
            return newDamage;
        }

        private void OnResisted()
        {
            if (Resisted != null)
                Resisted();
        }

        public event DEFAULT_HANDLER Resisted;
    }
}