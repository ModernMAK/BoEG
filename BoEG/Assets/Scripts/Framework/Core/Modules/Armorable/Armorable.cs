using System;
using Framework.Types;

namespace Framework.Core.Modules
{
    public class Armorable : Module, IArmorable
    {
        public ArmorData Physical { get; private set; }
        public ArmorData Magical { get; private set; }


        private float CalculateReduction(float value, DamageType type)
        {
            switch (type)
            {
                case DamageType.Physical:
                    return Physical.CalculateReduction(value);
                case DamageType.Magical:
                    return Magical.CalculateReduction(value);
                case DamageType.Pure:
                case DamageType.Modification:
                    return 0f;
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }
        }

        private float CalculateDamageAfterReduction(float value, DamageType type)
        {
            var reduction = CalculateReduction(value, type);
            return value - reduction;
        }

        private Damage CalculateDamageAfterReduction(Damage damage)
        {
            return damage.SetValue(CalculateDamageAfterReduction(damage.Value, damage.Type));
        }

        public Damage ResistDamage(Damage damage)
        {
            var args = CreateArgs(damage);
            OnResistingDamage(args);
            var dam = CalculateDamageAfterReduction(damage);
            OnResistedDamage(args);
            return dam;
        }


        public event ResistEventHandler ResistingDamage;
        public event ResistEventHandler ResistedDamage;

        private ResistEventArgs CreateArgs(Damage damage)
        {
            return new ResistEventArgs(damage, CalculateDamageAfterReduction(damage));
        }

        private void OnResistingDamage(ResistEventArgs args)
        {
            if (ResistingDamage != null)
                ResistingDamage(args);
        }
        private void OnResistedDamage(ResistEventArgs args)
        {
            if (ResistedDamage != null)
                ResistedDamage(args);
        }

        private IArmorableData _armorableData;

        protected override void Spawn()
        {
            GetData(out _armorableData);
            Physical = _armorableData.Physical;
            Magical = _armorableData.Magical;
        }
    }
}