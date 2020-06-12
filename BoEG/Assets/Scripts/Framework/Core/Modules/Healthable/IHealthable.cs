using System;

namespace Framework.Core.Modules
{
    public interface IHealthable
    {
        float Health { get; set; }
        float HealthPercentage { get; set; }
        float HealthCapacity { get; set; }
        float HealthGeneration { get; set; }

        event EventHandler<float> HealthChanged;
    }
}