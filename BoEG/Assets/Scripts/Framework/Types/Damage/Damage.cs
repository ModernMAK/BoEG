using System;
using UnityEngine;

namespace Framework.Types
{
    public struct Damage
    {
        public Damage(float value, DamageType type, params DamageModifiers[] modifiers)
        {
            if (value < 0)
                throw new ArgumentException("Cannot be less than 0.",nameof(value));
            
            Modifiers = MergerModifiers(modifiers);
            Type = type;
            Value = value;
        }

        private Damage(Damage damage)
        {
            Value = damage.Value;
            Modifiers = damage.Modifiers;
            Type = damage.Type;
        }
        
        public float Value { get; private set; }
        public DamageModifiers Modifiers { get; private set; }
        
        public DamageType Type { get; private set; }

        public Damage SetValue(float value)
        {
            if (value < 0f)
                value = 0f;
            return new Damage(this)
            {                
                Value = value,
            };
        }
        public Damage ModifyValue(float change)
        {
            return SetValue(Value + change);
        }
        public Damage SetType(DamageType type)
        {
            return new Damage(this)
            {
                Type = type
            };
        }
        public Damage SetModifiers(params DamageModifiers[] modifiers)
        {
            return new Damage(this)
            {
                Modifiers = MergerModifiers(modifiers)
            };
        }
        public Damage AddModifiers(params DamageModifiers[] modifiers)
        {
            return new Damage(this)
            {
                Modifiers = Modifiers | MergerModifiers(modifiers)
            };
        }
        public Damage RemoveModifiers(params DamageModifiers[] modifiers)
        {
            return new Damage(this)
            {
                Modifiers = Modifiers & ~MergerModifiers(modifiers)
            };
        }

        private static DamageModifiers MergerModifiers(DamageModifiers[] modifiers)
        {
            
            var modifier = (DamageModifiers)0;
            foreach (var mod in modifiers)
                modifier |= mod;
            return modifier;
        }
        public Damage Duplicate()
        {
            return new Damage(this);
        }
    }
//
//    public struct Damage
//    {
//        public Damage(float damageValue, DamageType damageType, GameObject source) : this()
//        {
//            Value = damageValue;
//            Type = damageType;
//            Source = source;
//        }
//
//        public Damage(Damage damage) : this(damage.Value, damage.Type, damage.Source)
//        {
//        }
//
//        public GameObject Source { get; private set; }
//        public float Value { get; private set; }
//        public DamageType Type { get; private set; }
//
//        public Damage SetSource(GameObject source)
//        {
//            return new Damage(this)
//            {
//                Source = source
//            };
//        }
//
//        public Damage SetValue(float value)
//        {
//            return new Damage(this)
//            {
//                Value = value
//            };
//        }
//
//        public Damage SetType(DamageType type)
//        {
//            return new Damage(this)
//            {
//                Type = type
//            };
//        }
//    }
}