using System.Collections.Generic;
using Framework.Core.Modules;
using Framework.Types;
using UnityEngine;

namespace Framework.Core
{
    public class Actor : MonoBehaviour
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
        }

        protected virtual void Update()
        {
            foreach (var item in _steppable) item.Step(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            foreach (var item in _steppable) item.PhysicsStep(Time.fixedDeltaTime);
        }

        protected virtual void LateUpdate()
        {
            foreach (var item in _steppable) item.PostStep(Time.deltaTime);

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
    }
}