using Framework.Types;

namespace Framework.Core.Modules.Commands
{
    public abstract class Command : ICommand
    {
        
        protected virtual void PreStep(float delta)
        {
        }

        void IStepable.PreStep(float delta)
        {
            PreStep(delta);
        }

        protected virtual void Step(float delta)
        {
        }

        void IStepable.Step(float delta)
        {
            Step(delta);
        }

        protected virtual void PostStep(float delta)
        {
        }

        void IStepable.PostStep(float delta)
        {
            PostStep(delta);
        }

        protected virtual void PhysicsStep(float delta)
        {
        }

        void IStepable.PhysicsStep(float delta)
        {
            PhysicsStep(delta);
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