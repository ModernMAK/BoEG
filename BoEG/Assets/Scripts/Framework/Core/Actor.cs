using System;
using System.Collections.Generic;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core
{
    public class Actor : MonoBehaviour, IStepableEvent
    {
        public virtual Sprite GetIcon() => null;

        private List<IStepable> _steppable;

        protected virtual void Awake()
        {
            _steppable = new List<IStepable>();
            SetupComponents();
        }

        protected virtual void SetupComponents()
        {
            var steppables = GetComponents<IStepable>();
            _steppable.AddRange(steppables);
            var steppableListener = GetComponents<IListener<IStepableEvent>>();
            foreach (var steppable in steppableListener)
            {
                steppable.Register(this);
            }
        }

        protected virtual void Update()
        {
            OnStep(Time.deltaTime);
            foreach (var item in _steppable) item.Step(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            OnPhysicsStep(Time.deltaTime);
            foreach (var item in _steppable) item.PhysicsStep(Time.fixedDeltaTime);
        }

        protected virtual void LateUpdate()
        {
            OnPostStep(Time.deltaTime);
            foreach (var item in _steppable) item.PostStep(Time.deltaTime);

            OnPreStep(Time.deltaTime);
            foreach (var item in _steppable) item.PreStep(Time.deltaTime);
        }

        public void AddSteppable(IStepable steppable)
        {
            _steppable.Add(steppable);
        }

        public void RemoveSteppable(IStepable steppable)
        {
            _steppable.Remove(steppable);
        }

        public void AddSteppable(IListener<IStepableEvent> stepableEvent) => stepableEvent.Register(this);
        public void RemoveSteppable(IListener<IStepableEvent> stepableEvent) => stepableEvent.Unregister(this);

        //Todo move to a more appropriate lcoation
        protected IInitializable<T> GetInitializable<T>()
        {
            return GetComponent<IInitializable<T>>();
        }

        protected bool TryGetInitializable<T>(out IInitializable<T> initializable)
        {
            initializable = GetInitializable<T>();
            return initializable != null;
        }

        private event Action<float> _step;
        private event Action<float> _preStep;
        private event Action<float> _postStep;
        private event Action<float> _physicsStep;

        public event Action<float> PreStep
        {
            add => _preStep += value;
            remove => _preStep -= value;
        }

        public event Action<float> Step
        {
            add => _step += value;
            remove => _step -= value;
        }

        public event Action<float> PostStep
        {
            add => _postStep += value;
            remove => _postStep -= value;
        }

        public event Action<float> PhysicsStep
        {
            add => _physicsStep += value;
            remove => _physicsStep -= value;
        }

        protected virtual void OnStep(float obj)
        {
            _step?.Invoke(obj);
        }

        protected virtual void OnPreStep(float obj)
        {
            _preStep?.Invoke(obj);
        }

        protected virtual void OnPostStep(float obj)
        {
            _postStep?.Invoke(obj);
        }

        protected virtual void OnPhysicsStep(float obj)
        {
            _physicsStep?.Invoke(obj);
        }
    }
}