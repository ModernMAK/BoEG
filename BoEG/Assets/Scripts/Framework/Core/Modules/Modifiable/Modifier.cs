namespace MobaGame.Framework.Core.Modules
{
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

}