using System;

namespace Framework.Core.Modules
{
    public interface IHealthable
    {
        float Health { get; }
        float HealthPercentage { get; }
        float HealthCapacity { get; }
        float HealthGeneration { get; }
        void ModifyHealth(float change);
        void SetHealth(float health);
        event EventHandler<HealthableEventArgs> Modified;
        event EventHandler<HealthableEventArgs> Modifying;
    }
}