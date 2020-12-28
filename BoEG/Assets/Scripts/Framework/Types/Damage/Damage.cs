using System;

namespace Framework.Types
{
    public readonly struct SourcedDamage<TSource>
    {
        public SourcedDamage(Damage damage, TSource source)
        {
            Damage = damage;
            Source = source;
        }

        public Damage Damage { get; }
        public TSource Source { get; }

        public SourcedDamage<TSource> SetDamage(Damage damage) => new SourcedDamage<TSource>(damage, Source);
        public SourcedDamage<TSource> SetSource(TSource source) => new SourcedDamage<TSource>(Damage, source);
    }

    /// <summary>
    ///     A Damage Instance
    /// </summary>
    public readonly struct Damage

    {
        private const bool CLAMP_DEFAULT = false;
        private const string NegativeDamageErrorMessage = "Damage Value cannot be negative!";

        private static void ValidateValue(float value)
        {
            ValidateValue(ref value, false);
        }

        private static void ValidateValue(ref float value, bool clamp = CLAMP_DEFAULT)
        {
            if (value >= 0f) return;
            if (clamp)
                value = 0f;
            else
                throw new ArgumentOutOfRangeException(nameof(value), value, NegativeDamageErrorMessage);
        }

        //Copy constructor
        public Damage(Damage damage)
        {
            Value = damage.Value;
            Modifiers = damage.Modifiers;
            Type = damage.Type;
        }

        //Standard (No or One Modifier)
        public Damage(float value, DamageType type, DamageModifiers modifiers = DamageModifiers.None)
        {
            ValidateValue(value);
            Modifiers = modifiers;
            Type = type;
            Value = value;
        }

        //Modifier List Constructor
        public Damage(float value, DamageType type, params DamageModifiers[] modifiers) :
            this(value, type, MergeModifiers(modifiers))
        {
        }

        public float Value { get; }
        public DamageModifiers Modifiers { get; }
        public DamageType Type { get; }

        public Damage SetValue(float value, bool clamp = CLAMP_DEFAULT)
        {
            ValidateValue(ref value, clamp);
            return new Damage(value, Type, Modifiers);
        }

        public Damage ModifyValue(float change, bool clamp = CLAMP_DEFAULT)
        {
            return SetValue(Value + change, clamp);
        }

        public Damage SetType(DamageType type)
        {
            return new Damage(Value, type, Modifiers);
        }

        public Damage SetModifiers(params DamageModifiers[] modifiers)
        {
            return new Damage(Value, Type, modifiers);
        }

        public Damage SetModifiers(DamageModifiers modifier)
        {
            return new Damage(Value, Type, modifier);
        }

        public Damage AddModifiers(params DamageModifiers[] modifiers)
        {
            var merged = Modifiers | MergeModifiers(modifiers);
            return SetModifiers(merged);
        }

        public Damage RemoveModifiers(params DamageModifiers[] modifiers)
        {
            var merged = Modifiers & ~MergeModifiers(modifiers);
            return SetModifiers(merged);
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