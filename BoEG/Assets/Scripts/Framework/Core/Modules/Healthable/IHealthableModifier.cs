using System;
using System.Collections.Generic;

namespace MobaGame.Framework.Core.Modules
{
    public static class ModifierX
    {
        public static Modifier SumModifiers<T>(this IEnumerable<T> enumerable, Func<T,Modifier> getter) where T : IModifier
		{
            var result = new Modifier(0, 0, 0);
            foreach(var item in enumerable)
			{
                var modifier = getter(item);
                result = result.AddModifier(modifier);
			}
            return result;

		}
    }
    public struct Modifier
    {
        public Modifier(float bonus, float addMul = default, float mulMul = default) : this(bonus, new ModifierMultiplier(addMul,mulMul))        {        }
        public Modifier(float bonus, ModifierMultiplier multiplier)
		{
            Bonus = bonus;
            Multiplier = multiplier;
		}

        public float Bonus { get; }

        public ModifierMultiplier Multiplier { get; }

        public float Calculate(float value) => Bonus + Multiplier.Calculate(value);

        public Modifier AddModifier(Modifier modifier) => new Modifier(Bonus + modifier.Bonus, Multiplier.AddModifier(modifier.Multiplier));
		
	}
    public struct ModifierMultiplier
	{
        public ModifierMultiplier(float add, float mul)
		{
            Additive = add;
            Multiplicative = mul;
		}

        public   float Additive { get; }
        public float Multiplicative { get; }
        public float Calculate(float value) => value * (1 + Additive + Multiplicative);
        public ModifierMultiplier AddModifier(ModifierMultiplier modifier) => new ModifierMultiplier(Additive + modifier.Additive, (1 + Multiplicative) * (1 + modifier.Multiplicative) - 1);
    }
    public interface IHealthCapacityModifier : IModifier
	{
        Modifier HealthCapacity { get; }
    }
    public interface IHealthGenerationModifier : IModifier
	{
        Modifier HealthGeneration { get; }
	}

    public interface IMagicCapacityModifier : IModifier
    {
        Modifier MagicCapacity { get; }
    }
    public interface IMagicGenerationModifier : IModifier
    {
        Modifier MagicGeneration { get; }
    }

}