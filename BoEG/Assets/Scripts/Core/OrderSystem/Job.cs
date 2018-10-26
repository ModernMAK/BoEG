using UnityEngine;

namespace Core.OrderSystem
{
    public class Job : IJob
    {
        public virtual void Initialize(GameObject entity)
        {
        }

        //A Job only terminates after being initialized
        public virtual void Terminate()
        {
        }


        public virtual void PreStep(float deltaStep)
        {
        }

        public virtual void Step(float deltaTick)
        {
        }

        public virtual void PostStep(float deltaTick)
        {
        }
        public virtual void PhysicsStep(float deltaTick)
        {
        }

        public virtual bool IsDone()
        {
            return false;
        }
    }
}