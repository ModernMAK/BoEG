using Old.Entity.Modules.Buffable;

namespace Old.Entity.Modules.Healthable
{
    public interface IHealthableBuffInstance : IBuffInstance
    {
        float HealthCapacityBonus { get; }
        float HealthCapacityMultiplier { get; }
        float HealthGenBonus { get; }
        float HealthGenMultiplier { get; }
    }
}