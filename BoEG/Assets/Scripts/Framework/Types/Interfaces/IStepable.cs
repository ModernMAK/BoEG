using System;

namespace MobaGame.Framework.Types
{
    public interface IStepable
    {
        void PreStep(float deltaTime);
        void Step(float deltaTime);
        void PostStep(float deltaTime);
        void PhysicsStep(float deltaTime);
    }

    public static class ISteppableX
    {
        public static void Register(this IStepableEvent stepEvent, IStepable stepable)
        {
            stepEvent.PreStep += stepable.PreStep;
            stepEvent.PostStep += stepable.PostStep;
            stepEvent.Step += stepable.Step;
            stepEvent.PhysicsStep += stepable.PhysicsStep;
        }
        public static void Unregister(this IStepableEvent stepEvent, IStepable stepable)
        {
            stepEvent.PreStep -= stepable.PreStep;
            stepEvent.PostStep -= stepable.PostStep;
            stepEvent.Step -= stepable.Step;
            stepEvent.PhysicsStep -= stepable.PhysicsStep;
        }
        public static void Register(this IStepable stepable, IStepableEvent stepEvent) => stepEvent.Register(stepable);

        public static void Unregister(this IStepable stepable, IStepableEvent stepEvent) => stepEvent.Unregister(stepable);
    }


    public interface IStepableEvent
    {
        event Action<float> PreStep;
        event Action<float> Step;
        event Action<float> PostStep;
        event Action<float> PhysicsStep;
    }
}