using Core;

namespace Modules.Healthable
{
    public interface IHealthableData
    {
        FloatScalar HealthCapacity { get; }
        FloatScalar HealthGeneration { get; }
    }
}