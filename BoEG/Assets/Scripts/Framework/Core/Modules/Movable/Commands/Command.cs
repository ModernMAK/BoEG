using Framework.Types;

namespace Framework.Core.Modules.Commands
{
    public abstract class Command : ICommand
    {
        
        protected virtual void PreStep(float deltaTime)
        {
        }

        void IStepable.PreStep(float deltaTime)
        {
            PreStep(deltaTime);
        }

        protected virtual void Step(float deltaTime)
        {
        }

        void IStepable.Step(float deltaTime)
        {
            Step(deltaTime);
        }

        protected virtual void PostStep(float deltaTime)
        {
        }

        void IStepable.PostStep(float deltaTime)
        {
            PostStep(deltaTime);
        }

        protected virtual void PhysicsStep(float deltaTime)
        {
        }

        void IStepable.PhysicsStep(float deltaTime)
        {
            PhysicsStep(deltaTime);
        }

        void ICommand.Start()
        {
            Start();
        }

        protected virtual void Start()
        {
        }

        void ICommand.Stop()
        {
            Stop();
        }

        protected virtual void Stop()
        {
        }

        bool ICommand.IsDone()
        {
            return IsDone();
        }

        protected virtual bool IsDone()
        {
            return true;
        }
    }
}