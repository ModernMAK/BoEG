namespace Framework.Core.Modules
{
    public interface IHealthableData
    {
        float HealthGeneration { get; }
        float HealthCapacity { get; }
        float HealthGenerationGain { get; }
        float HealthCapacityGain { get; }

    }
}