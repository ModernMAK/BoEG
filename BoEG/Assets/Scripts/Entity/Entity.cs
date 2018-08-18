using System.Collections.Generic;
using System.Linq;
using Modules;
using Triggers;
using UnityEngine;
using UnityEngine.Networking;

namespace Entity
{
    public class Entity : NetworkBehaviour, ITickable
    {
        protected virtual IEnumerable<IModule> Modules
        {
            get { return Enumerable.Empty<IModule>(); }
        }

        protected virtual void Awake()
        {
            SetLayerRecursive(gameObject, (int) Layer.Entity);
        }

        protected virtual void OnEnable()
        {
            EntityRegistry.Register(gameObject);
            Initialize();
        }

        protected virtual void OnDisable()
        {
            EntityRegistry.Deregister(gameObject);
            Terminate();
        }

        public static void SetLayerRecursive(GameObject root, int layer)
        {
            root.layer = layer;
            foreach (Transform child in root.transform)
            {
                SetLayerRecursive(child.gameObject, layer);
            }
        }

        private float _lastPreTick, _lastPostTick, _lastTick, _lastPhysicsTick;

        protected virtual void Update()
        {
            var deltaTick = Time.time - _lastTick;
            _lastTick = Time.time;
            Tick(deltaTick);
        }

        protected virtual void LateUpdate()
        {
            var deltaTick = Time.time - _lastPostTick;
            _lastPostTick = Time.time;
            PostTick(deltaTick);

            deltaTick = Time.time - _lastPreTick;
            _lastPreTick = Time.time;
            PreTick(deltaTick);
        }

        private void FixedUpdate()
        {
            var deltaTick = Time.time - _lastPhysicsTick;
            _lastPhysicsTick = Time.time;
            PhysicsTick(deltaTick);
        }

//        private void PreTick()
//        {
//            foreach (var module in Modules)
//            {
//                module.PreTick();
//            }
//        }
//
//        private void Tick()
//        {
//            foreach (var module in Modules)
//            {
//                module.Tick();
//            }
//        }
//
//        private void PostTick()
//        {
//            foreach (var module in Modules)
//            {
//                module.PostTick();
//            }
//        }

//        public void PhysicsTick(float deltaTick)
//        {
//            foreach (var module in Modules)
//            {
//                module.PhysicsTick(deltaTick);
//            }
//        }

        private void Initialize()
        {
            foreach (var module in Modules)
            {
                module.Initialize();
            }
        }
        private void Terminate()
        {
            foreach (var module in Modules)
            {
                module.Terminate();
            }
        }
        public void PreTick(float deltaTick)
        {            
            Modules.PreTick(deltaTick);
        }

        public void Tick(float deltaTick)
        {
            Modules.Tick(deltaTick);
        }

        public void PostTick(float deltaTick)
        {
            Modules.PostTick(deltaTick);
        }

        public void PhysicsTick(float deltaTick)
        {
            Modules.PhysicsTick(deltaTick);
        }
		
		public byte[] Serialize()
		{
			using(var serializer = new Serializer())
			{
				Modules.Serialize(serializer);
				return serializer.Output();
			}
		}
		public void Deserialize(byte[] serialized)
		{
			using(var deserializer = new Deserializer(serialized))
			{
				Modules.Deserialize(serializer);
			}
		}
    }
}