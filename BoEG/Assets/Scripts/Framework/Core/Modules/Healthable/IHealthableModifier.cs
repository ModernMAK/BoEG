namespace MobaGame.Framework.Core.Modules
{
    public interface IHealthableModifier : IModifier
    {
        float HealthCapacityBonus { get; }
        float HealthCapacityFlatMultiplier { get; }
        float HealthCapacityMultiplicativeMultiplier { get; }
        float HealthGenerationBonus { get; }
        float HealthGenerationFlatMultiplier { get; }
        float HealthGenerationMultiplicativeMultiplier { get; }
    }
}