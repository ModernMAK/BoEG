namespace Entity
{
    public interface IStepable
    {
        void PreStep(float deltaStep);
        void Step(float deltaTick);
        void PostStep(float deltaTick);
        void PhysicsStep(float deltaTick);
    }
}