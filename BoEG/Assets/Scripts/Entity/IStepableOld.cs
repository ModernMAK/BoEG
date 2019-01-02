using System;

namespace Entity
{
    [Obsolete]
    public interface IStepableOld
    {
        void PreStep(float deltaStep);
        void Step(float deltaTick);
        void PostStep(float deltaTick);
        void PhysicsStep(float deltaTick);
    }
}