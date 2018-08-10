using System;
using System.Linq;
using Core;
using UnityEngine;

namespace Old.Entity.Modules.Armorable
{
    [Serializable]
    public class Armorable : FullModule, IArmorable
    {
        protected override void Initialize()
        {
            _data = GetData<IArmorableData>();
        }

        /// <summary>
        /// The backing field containing template data for Armorable.
        /// </summary>
        [SerializeField] private IArmorableData _data;

        public float PhysicalBlock
        {
            get
            {
                var baseVal = _data.BasePhysicalBlock;
                var gainVal = _data.GainPhysicalBlock;
                Func<IArmorableBuffInstance, float> func = (x => x.PhysicalBlockBonus);

                return this.CalculateValueBonus(baseVal, gainVal, func);
            }
        }

        public float PhysicalResist
        {
            get
            {
                var baseVal = _data.BasePhysicalResist;
                var gainVal = _data.GainPhysicalResist;
                Func<IArmorableBuffInstance, float> func = (x => x.PhysicalResistkMultiplier);

                return this.CalculateResist(baseVal, gainVal, func);
            }
        }

        public bool PhysicalImmunity
        {
            get
            {
                var baseVal = _data.HasPhysicalImmunity;
                if (baseVal) return true;
                var buffs = GetBuffs<IArmorableBuffInstance>();
                baseVal = buffs.Any(buff => buff.ProvidePhysicalImmunity);
                return baseVal;
            }
        }

        public float MagicalBlock
        {
            get
            {
                var baseVal = _data.BaseMagicalBlock;
                var gainVal = _data.GainMagicalBlock;
                Func<IArmorableBuffInstance, float> func = (x => x.MagicalBlockBonus);

                return this.CalculateValueBonus(baseVal, gainVal, func);
            }
        }

        public float MagicalResist
        {
            get
            {
                var baseVal = _data.BaseMagicalResist;
                var gainVal = _data.GainMagicalResist;
                Func<IArmorableBuffInstance, float> func = (x => x.MagicalResistkMultiplier);

                return this.CalculateResist(baseVal, gainVal, func);
            }
        }

        public bool MagicalImmunity
        {
            get
            {
                var baseVal = _data.HasMagicalImmunity;
                if (baseVal) return true;
                var buffs = GetBuffs<IArmorableBuffInstance>();
                baseVal = buffs.Any(buff => buff.ProvideMagicalImmunity);
                return baseVal;
            }
        }

        public static float ReductionCalculation(float damage, float block = 0f, float resist = 0f,
            bool immunity = false)
        {
            if (immunity)
                return 0f;
            //A positive resistance negates damage
            //Ergo, we subtract resistance
            damage = (damage - block) * (1f - resist);
            return Mathf.Max(damage, 0f);
        }

        public float CalculateDamageAfterReductions(Damage damage)
        {
            //block first, then resist
            //D(d) = (d-B)*(1-R)
            switch (damage.Type)
            {
                case DamageType.Physical:
                    return ReductionCalculation(damage.Value, PhysicalBlock, PhysicalResist, PhysicalImmunity);
                case DamageType.Magical:
                    return ReductionCalculation(damage.Value, MagicalBlock, MagicalResist, MagicalImmunity);
                case DamageType.Pure:
                case DamageType.Modification:
                    return damage.Value;
                default:
                    return damage.Value;
            }
        }

        [Obsolete("Passing by damage-type is depriciated; use AttackDamage struct instead")]
        public float CalculateDamageAfterReductions(float damage, DamageType type)
        {
            //block first, then resist
            //D(d) = (d-B)*(1-R)
            switch (type)
            {
                case DamageType.Physical:
                    return ReductionCalculation(damage, PhysicalBlock, PhysicalResist, PhysicalImmunity);
                case DamageType.Magical:
                    return ReductionCalculation(damage, MagicalBlock, MagicalResist, MagicalImmunity);
                case DamageType.Pure:
                case DamageType.Modification:
                    return damage;
                default:
                    return damage;
            }
        }

        public Damage ResistDamage(Damage damage)
        {
            var newDamage = CalculateDamageAfterReductions(damage);
            var resisted = damage.Value - newDamage;
            OnResisted(new ResistEventArgs(resisted, damage.Type, damage.Source));
            return new Damage(newDamage, damage.Type, damage.Source);
        }

        private void OnResisted(ResistEventArgs resistArgs)
        {
            if (Resisted != null)
                Resisted(resistArgs);
        }

        public event ResistHandler Resisted;
    }
}