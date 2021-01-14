using System;
using System.Collections.Generic;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core
{
    public static class ActorUtil
    {
        public static T GetModuleOrComponent<T>(this Actor actor)
        {
            if (actor.TryGetModule<T>(out var module))
                return module;
            if (actor.TryGetComponent<T>(out var component))
                return component;
            return default;
        }

        public static bool TryGetModuleOrComponent<T>(this Actor actor, out T module)
        {
            if (actor.TryGetModule<T>(out var moduleResult))
            {
                module = moduleResult;
                return true;
            }

            if (actor.TryGetComponent<T>(out var componentResult))
            {
                module = componentResult;
                return true;
            }

            module = default;
            return default;
        }

        public static IEnumerable<T> GetModules<T>(this Actor actor)
        {
            var modules = actor.GetModules<T>();
            foreach (var module in modules)
            {
                yield return module;
            }

            var components = actor.GetComponents<T>();
            foreach (var component in components)
            {
                yield return component;
            }
        }

        public static IReadOnlyList<T> GetModulesAsList<T>(this Actor actor)
        {
            var modules = actor.GetModules<T>();
            var components = actor.GetComponents<T>();
            var list = new List<T>(components);
            list.AddRange(modules);
            return list;
        }
    }

    public class Actor : MonoBehaviour, IStepableEvent
    {
        public virtual Sprite GetIcon() => null;

        private List<IStepable> _steppable;


        public T GetModule<T>()
        {
            TryGetModule<T>(out var module);
            return module;
        }

        public bool TryGetModule<T>(out T module)
        {
            foreach (var m in Modules)
            {
                if (m is T result)
                {
                    module = result;
                    return true;
                }
            }

            module = default;
            return false;
        }

        public IEnumerable<T> GetModules<T>()
        {
            foreach (var module in Modules)
            {
                if (module is T result)
                {
                    yield return result;
                }
            }
        }

        public IReadOnlyList<T> GetModulesAsList<T>()
        {
            var list = new List<T>();
            foreach (var module in Modules)
            {
                if (module is T result)
                {
                    list.Add(result);
                }
            }

            return list;
        }


        protected virtual IEnumerable<object> Modules
        {
            get { yield break; }
        }

        protected virtual void Awake()
        {
            _steppable = new List<IStepable>();
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
            _steppable.AddRange(steppables);
            var steppableListener = GetComponents<IListener<IStepableEvent>>();
            foreach (var steppable in steppableListener)
            {
                steppable.Register(this);
            }

            var childSteppables = GetModules<IListener<IStepableEvent>>();
            foreach (var steppable in childSteppables)
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

        [Obsolete("Just use Register")]
        public void AddSteppable(IListener<IStepableEvent> stepableEvent) => stepableEvent.Register(this);


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