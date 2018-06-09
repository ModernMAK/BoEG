namespace Components.Healthable
{
    public interface IHealthable
    {
        float HealthPoints { get; set; }
        float HealthRatio { get; set; }
        float HealthCapacity { get; }
        float HealthGen { get; }
    }
}