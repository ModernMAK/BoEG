namespace MobaGame.Framework.Core.Modules
{
    public interface IMagicableModifier : IModifier
    {
        float MagicCapacityBonus { get; }
        float MagicCapacityFlatMultiplier { get; }
        float MagicCapacityMultiplicativeMultiplier { get; }
        float MagicGenerationBonus { get; }
        float MagicGenerationFlatMultiplier { get; }
        float MagicGenerationMultiplicativeMultiplier { get; }
    }
}