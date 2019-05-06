using System;
using Entity;
using Framework.Types;
using UnityEngine;

namespace Framework.Core.Modules
{
    
    
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

        void IStepable.PreStep(float deltaTime)
        {
            PreStep(deltaTime);
        }

        protected virtual void PreStep(float deltaTime)
        {
        }

        void IStepable.Step(float deltaTime)
        {
            Step(deltaTime);
        }

        protected virtual void Step(float deltaStep)
        {
        }

        void IStepable.PostStep(float deltaTime)
        {
            PostStep(deltaTime);
        }

        protected virtual void PostStep(float deltaStep)
        {
        }

        void IStepable.PhysicsStep(float deltaTime)
        {
            PhysicsStep(deltaTime);
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


        protected bool IsModuleReady
        {
            get { return IsInitialized && IsSpawned; }
        }

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