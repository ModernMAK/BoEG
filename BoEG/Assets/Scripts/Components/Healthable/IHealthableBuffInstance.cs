using Components.Buffable;

namespace Components.Healthable
{
    public interface IHealthableBuffInstance : IBuffInstance
    {
        float HealthCapacityBonus { get; }
        float HealthCapacityMultiplier { get; }
        float HealthGenBonus { get; }
        float HealthGenMultiplier { get; }
    }
}