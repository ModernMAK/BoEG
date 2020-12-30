using Framework.Types;

namespace Framework.Core.Modules.Commands
{
    public abstract class BaseCommand : ICommand
    {
        void IStepable.PreStep(float deltaTime)
        {
            PreStep(deltaTime);
        }

        void IStepable.Step(float deltaTime)
        {
            Step(deltaTime);
        }

        void IStepable.PostStep(float deltaTime)
        {
            PostStep(deltaTime);
        }

        void IStepable.PhysicsStep(float deltaTime)
        {
            PhysicsStep(deltaTime);
        }

        void ICommand.Start()
        {
            Start();
        }

        void ICommand.Stop()
        {
            Stop();
        }

        bool ICommand.IsDone()
        {
            return IsDone();
        }

        protected virtual void PreStep(float deltaTime)
        {
        }

        protected virtual void Step(float deltaTime)
        {
        }

        protected virtual void PostStep(float deltaTime)
        {
        }

        protected virtual void PhysicsStep(float deltaTime)
        {
        }

        protected abstract void Start();

        protected abstract void Stop();

        protected abstract bool IsDone();
    }
}