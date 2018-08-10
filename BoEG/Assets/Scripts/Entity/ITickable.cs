namespace Entity
{
    public interface ITickable
    {
        void PreTick(float deltaTick);
        void Tick(float deltaTick);
        void PostTick(float deltaTick);
        void PhysicsTick(float deltaTick);
    }
}