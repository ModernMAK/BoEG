﻿using MobaGame.Framework.Core;
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