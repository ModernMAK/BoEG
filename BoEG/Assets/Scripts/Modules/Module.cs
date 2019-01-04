//using Framework.Core.Serialization;
//using UnityEngine;
//
//namespace Modules
//{
//    public class Module : IModule
//    {
//        protected GameObject Self { get; private set; }        
//        protected Module(GameObject self)
//        {
//            Self = self;
//        }
//
//        public virtual void Initialize()
//        {
//        }        
//        public virtual void PreStep(float deltaStep)
//        {
//        }
//
//        public virtual void Step(float deltaTick)
//        {
//        }
//
//        public virtual void PostStep(float deltaTick)
//        {
//        }
//        public virtual void PhysicsStep(float deltaTick)
//        {
//        }
//        public virtual void Terminate()
//        {
//        }
//		
//		public virtual void Serialize(ISerializer serializer)
//		{
//			
//		}
//		public virtual void Deserialize(IDeserializer deserializer)
//		{
//			
//		}
//    }
//}