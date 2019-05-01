using System;
using Entity;
using Framework.Types;
using UnityEngine;

namespace Framework.Core.Modules
{
    public interface IModule : IStepable, ISpawnable, IInstantiable
    {
    }
    
    [Obsolete("Use IModule instead")]
    public abstract class Module : MonoBehaviour, IStepable, ISpawnable, IInstantiable
    {
        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
        }

        protected virtual void LateUpdate()
        {
        }

        protected virtual void FixedUpdate()
        {
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        void IStepable.PreStep(float deltaStep)
        {
            PreStep(deltaStep);
        }

        protected virtual void PreStep(float deltaStep)
        {
        }

        void IStepable.Step(float deltaTick)
        {
            Step(deltaTick);
        }

        protected virtual void Step(float deltaStep)
        {
        }

        void IStepable.PostStep(float deltaTick)
        {
            PostStep(deltaTick);
        }

        protected virtual void PostStep(float deltaStep)
        {
        }

        void IStepable.PhysicsStep(float deltaTick)
        {
            PhysicsStep(deltaTick);
        }

        protected virtual void PhysicsStep(float deltaStep)
        {
        }

        protected bool IsSpawned { get; private set; }

        void ISpawnable.Spawn()
        {
            Spawn();
            IsSpawned = true;
        }

        protected virtual void Spawn()
        {
        }

        void ISpawnable.Despawn()
        {
            Despawn();
            IsSpawned = false;
        }

        protected virtual void Despawn()
        {
        }

        protected bool IsInitialized { get; private set; }


        void IInstantiable.Instantiate()
        {
            Instantiate();
            IsInitialized = true;
        }

        protected virtual void Instantiate()
        {
        }

        void IInstantiable.Terminate()
        {
            Terminate();
            IsInitialized = false;
        }

        protected virtual void Terminate()
        {
        }

        protected T GetData<T>()
        {
            var comp = GetComponent<IInstantiableData<T>>();
            if(comp == null)
                throw new NullReferenceException($"Failed to find {typeof(IInstantiableData<T>)} component.");
            return comp.Data;
        }

        protected void GetData<T>(out T data)
        {
            data = GetData<T>();
        }
    }
}