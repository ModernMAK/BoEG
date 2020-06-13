using System;

namespace Framework.Types
{
    /// <summary>
    ///     A Damage Instance
    /// </summary>
    public struct Damage
    {
        public Damage(float value, DamageType type, params DamageModifiers[] modifiers)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), value, "Cannot be negative.");

            Modifiers = MergeModifiers(modifiers);
            Type = type;
            Value = value;
        }

        public Damage(Damage damage)
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
                throw new ArgumentOutOfRangeException(nameof(value), value, "Cannot be negative.");

            return new Damage(this)
            {
                Value = value
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
                Modifiers = MergeModifiers(modifiers)
            };
        }

        public Damage AddModifiers(params DamageModifiers[] modifiers)
        {
            return new Damage(this)
            {
                Modifiers = Modifiers | MergeModifiers(modifiers)
            };
        }

        public Damage RemoveModifiers(params DamageModifiers[] modifiers)
        {
            return new Damage(this)
            {
                Modifiers = Modifiers & ~MergeModifiers(modifiers)
            };
        }

        private static DamageModifiers MergeModifiers(DamageModifiers[] modifiers)
        {
            var modifier = (DamageModifiers) 0;
            foreach (var mod in modifiers)
                modifier |= mod;
            return modifier;
        }

        public Damage Duplicate()
        {
            return new Damage(this);
        }
    }
}