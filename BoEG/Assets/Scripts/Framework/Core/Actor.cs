using System;
using System.Collections.Generic;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core
{

	public class Actor : MonoBehaviour, IStepableEvent
    {
        public virtual Sprite GetIcon() => null;

        public T GetModule<T>() => EnumerableQuery.Get<T>(Modules);

        public bool TryGetModule<T>(out T module) => EnumerableQuery.TryGet<T>(Modules, out module);

        public IEnumerable<T> GetModules<T>()=> EnumerableQuery.GetAll<T>(Modules);

        public IReadOnlyList<T> GetModulesAsList<T>()=> EnumerableQuery.GetAllAsList<T>(Modules);


        protected virtual IEnumerable<object> Modules
        {
            get { yield break; }
        }

        protected virtual void Awake()
        {
            CreateComponents();
            SetupComponents();
        }

        protected virtual void Start()
        {
        }

        protected virtual void CreateComponents()
        {
        }

        protected virtual void SetupComponents()
        {
            var steppables = GetComponents<IStepable>();
            foreach (var steppable in steppables)
                AddSteppable(steppable);
            var steppableListener = GetComponents<IListener<IStepableEvent>>();
            foreach (var steppable in steppableListener)
                steppable.Register(this);

            var childSteppables = GetModules<IListener<IStepableEvent>>();
            foreach (var steppable in childSteppables)
                steppable.Register(this);
        }

        protected virtual void Update()
        {
            OnStep(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            OnPhysicsStep(Time.deltaTime);
        }

        protected virtual void LateUpdate()
        {
            OnPostStep(Time.deltaTime);

            OnPreStep(Time.deltaTime);
        }

        public void AddSteppable(IStepable steppable)
        {
            PreStep += steppable.PreStep;
            PostStep += steppable.PostStep;
            Step += steppable.Step;
            PhysicsStep += steppable.PhysicsStep;
        }
        public void RemoveSteppable(IStepable steppable)
        {
            PreStep -= steppable.PreStep;
            PostStep -= steppable.PostStep;
            Step -= steppable.Step;
            PhysicsStep -= steppable.PhysicsStep;
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