namespace MobaGame.Framework.Core.Modules
{
    public interface IMagicableData
    {
        float MagicGeneration { get; }
        float MagicCapacity { get; }
        float MagicGenerationGain { get; }
        float MagicCapacityGain { get; }
    }
}