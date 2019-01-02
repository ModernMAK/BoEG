namespace Framework.Types
{
    public interface IStepable
    {
        void PreStep(float delta);
        void Step(float delta);
        void PostStep(float delta);
        void PhysicsStep(float delta);
    }
}