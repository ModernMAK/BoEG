namespace MobaGame.Framework.Core.Modules
{
	public struct ModifierMultiplier
	{
        public ModifierMultiplier(float add, float mul)
		{
            Additive = add;
            Multiplicative = mul;
		}

        public   float Additive { get; }
        public float Multiplicative { get; }
        public float Calculate(float value) => value * (Additive + Multiplicative);
        public ModifierMultiplier AddModifier(ModifierMultiplier modifier) => new ModifierMultiplier(Additive + modifier.Additive, (1 + Multiplicative) * (1 + modifier.Multiplicative) - 1);
    }

}