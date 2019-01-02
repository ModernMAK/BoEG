namespace Framework.Core.Modules
{
    public interface IHealthable
    {
        
        bool IsDead { get; }
        PointData Health { get; }


        void ModifyHealth(float deltaValue);
        
        event HealthChangeHandler HealthModifying;
        event HealthChangeHandler HealthModified;

        event DeathHandler Died;
    }
}