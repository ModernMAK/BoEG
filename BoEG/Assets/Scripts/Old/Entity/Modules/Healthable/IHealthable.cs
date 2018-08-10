using Core;

namespace Old.Entity.Modules.Healthable
{
    public interface IHealthable
    {
        float HealthPoints { get; }
        float HealthRatio { get; }
        float HealthCapacity { get; }
        float HealthGen { get; }
        void TakeDamage(Damage damage);
        void ModifyHealth(float modified);
    }
}