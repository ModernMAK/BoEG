using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using System;

namespace MobaGame.Framework.Types
{
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
            Flags = damage.Flags;
            Type = damage.Type;
        }

        //Standard (No or One Modifier)
        public Damage(float value, DamageType type, DamageFlags flags = DamageFlags.None)
        {
            ValidateValue(value);
            Flags = flags;
            Type = type;
            Value = value;
        }

        //Modifier List Constructor
        public Damage(float value, DamageType type, params DamageFlags[] modifiers) :
            this(value, type, MergeModifiers(modifiers))
        {
        }

        public float Value { get; }
        public DamageFlags Flags { get; }
        public DamageType Type { get; }

        public Damage SetValue(float value, bool clamp = CLAMP_DEFAULT)
        {
            ValidateValue(ref value, clamp);
            return new Damage(value, Type, Flags);
        }

        public Damage ModifyValue(float change, bool clamp = CLAMP_DEFAULT)
        {
            return SetValue(Value + change, clamp);
        }

        public Damage SetType(DamageType type)
        {
            return new Damage(Value, type, Flags);
        }

        public Damage SetModifiers(params DamageFlags[] modifiers)
        {
            return new Damage(Value, Type, modifiers);
        }

        public Damage SetModifiers(DamageFlags flag)
        {
            return new Damage(Value, Type, flag);
        }

        public Damage AddModifiers(params DamageFlags[] modifiers)
        {
            var merged = Flags | MergeModifiers(modifiers);
            return SetModifiers(merged);
        }

        public Damage RemoveModifiers(params DamageFlags[] modifiers)
        {
            var merged = Flags & ~MergeModifiers(modifiers);
            return SetModifiers(merged);
        }

        private static DamageFlags MergeModifiers(DamageFlags[] modifiers)
        {
            var modifier = (DamageFlags) 0;
            foreach (var mod in modifiers)
                modifier |= mod;
            return modifier;
        }

        public Damage Duplicate()
        {
            return new Damage(this);
        }
    }

	public struct SourcedHeal : ISourcedValue<Actor, float>
    {
        public SourcedHeal(Actor source, float value)
        {
            Source = source;
            Value = value;
        }

        public Actor Source { get; }
        public float Value { get; }

        public SourcedHeal SetActor(Actor actor) => new SourcedHeal(actor, Value);
        public SourcedHeal SetHeal(float value) => new SourcedHeal(Source, value);

    }

    public interface ISourcedValue<TSource, TValue>
    {
        TSource Source { get; }
        TValue Value { get; }
    }
}