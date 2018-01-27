using UnityEngine;

namespace Trigger
{
    public class TriggerStrategy
    {
        public virtual bool ShouldEnter(GameObject go)
        {
            return false;
        }

        public virtual bool ShouldStay(GameObject go)
        {
            return false;
        }

        public virtual bool ShouldExit(GameObject go)
        {
            return false;
        }

        public virtual void Enter(GameObject go)
        {
        }

        public virtual void Stay(GameObject go)
        {
        }

        public virtual void Exit(GameObject go)
        {
        }
    }
}