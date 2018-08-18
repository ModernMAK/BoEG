using JetBrains.Annotations;
using UnityEngine;

namespace Modules
{
    public class Module : IModule
    {
        protected GameObject Self { get; private set; }        
        protected Module(GameObject self)
        {
            Self = self;
        }

        public virtual void Initialize()
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
        public virtual void Terminate()
        {
        }
		
		public virtual void Serialize(ISerializer serializer)
		{
			
		}
		public virtual void Deserialize(IDeserializer deserializer)
		{
			
		}
    }
}