using Core;

namespace Components.Healthable
{
    public interface IHealthableInstance
    {
        float HealthPoints { get; set; }
        float HealthRatio { get; set; }
        float HealthCapacity { get; }
        float HealthGen { get; }
    }
}