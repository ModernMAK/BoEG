using System;

namespace MobaGame.Framework.Types
{
    [Obsolete("In Most Cases, Use IStepableEvent Instead", false)]
    public interface IStepable
    {
        void PreStep(float deltaTime);
        void Step(float deltaTime);
        void PostStep(float deltaTime);
        void PhysicsStep(float deltaTime);
    }


    public interface IStepableEvent
    {
        event Action<float> PreStep;
        event Action<float> Step;
        event Action<float> PostStep;
        event Action<float> PhysicsStep;
    }
}