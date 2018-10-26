using System.Collections.Generic;
using System.Linq;
using Core.Serialization;
using Modules;
using Triggers;
using UnityEngine;
using UnityEngine.Networking;

namespace Entity
{
    //A Utility class, impliment to allow data to be set on the entity
    public interface IEntityData
    {
    }

    public class Entity : MonoBehaviour, IStepable
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

        public virtual void SetData(IEntityData data)
        {
            gameObject.SetActive(data != null); //If we set null, something is wrong, raise a warning
            if (data == null)
                Debug.LogWarning("Data is null, disabling gameObject.");
        }

        private float _lastPreTick, _lastPostTick, _lastTick, _lastPhysicsTick;

        protected virtual void Update()
        {
            var deltaTick = Time.time - _lastTick;
            _lastTick = Time.time;
            Step(deltaTick);
        }

        protected virtual void LateUpdate()
        {
            var deltaTick = Time.time - _lastPostTick;
            _lastPostTick = Time.time;
            PostStep(deltaTick);

            deltaTick = Time.time - _lastPreTick;
            _lastPreTick = Time.time;
            PreStep(deltaTick);
        }

        private void FixedUpdate()
        {
            var deltaTick = Time.time - _lastPhysicsTick;
            _lastPhysicsTick = Time.time;
            PhysicsStep(deltaTick);
        }

//        private void PreStep()
//        {
//            foreach (var module in Modules)
//            {
//                module.PreStep();
//            }
//        }
//
//        private void Step()
//        {
//            foreach (var module in Modules)
//            {
//                module.Step();
//            }
//        }
//
//        private void PostStep()
//        {
//            foreach (var module in Modules)
//            {
//                module.PostStep();
//            }
//        }

//        public void PhysicsStep(float deltaStep)
//        {
//            foreach (var module in Modules)
//            {
//                module.PhysicsStep(deltaStep);
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

        public void PreStep(float deltaStep)
        {
            Modules.PreStep(deltaStep);
        }

        public void Step(float deltaTick)
        {
            Modules.Step(deltaTick);
        }

        public void PostStep(float deltaTick)
        {
            Modules.PostStep(deltaTick);
        }

        public void PhysicsStep(float deltaTick)
        {
            Modules.PhysicsStep(deltaTick);
        }

        public byte[] Serialize()
        {
            using (var serializer = new Serializer())
            {
                Modules.Serialize(serializer);
                return serializer.Buffer();
            }
        }

        public void Deserialize(byte[] serialized)
        {
            using (var deserializer = new Deserializer(serialized))
            {
                Modules.Deserialize(deserializer);
            }
        }
    }
}