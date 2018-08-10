using Core;

namespace Modules.Healthable
{
    public interface IHealthable
    {
        float HealthPercentage { get; } //Same as Health Points
        float HealthPoints { get; } //Same as Health Percentage
        float HealthCapacity { get; }
        float HealthGeneration { get; }
        void ModifyHealth(float modification);
        event DEFAULT_HANDLER HealthModified;
        void TakeDamage(Damage damage);
        event DEFAULT_HANDLER DamageTaken;
        void Die();
        event DEFAULT_HANDLER Died;
    }
}