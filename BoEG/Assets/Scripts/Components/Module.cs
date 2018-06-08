using Core;
using UnityEngine.Networking;

namespace Components
{
    public class Module : IModule
    {
        public virtual void Initialize(Entity e)
        {
        }

        public virtual void PreTick()
        {
        }

        public virtual void Tick()
        {
        }

        public virtual void PostTick()
        {
        }

        public virtual bool IsDirty()
        {
            return false;
        }
        
        public virtual bool Serialize(NetworkWriter writer, bool initState)
        {
            return false;
        }

        public virtual void Deserialize(NetworkReader reader, bool initState)
        {
        }
    }
}