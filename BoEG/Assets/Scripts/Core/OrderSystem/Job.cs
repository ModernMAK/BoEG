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


        public virtual void PreTick(float deltaTick)
        {
        }

        public virtual void Tick(float deltaTick)
        {
        }

        public virtual void PostTick(float deltaTick)
        {
        }
        public virtual void PhysicsTick(float deltaTick)
        {
        }

        public virtual bool IsDone()
        {
            return false;
        }
    }
}