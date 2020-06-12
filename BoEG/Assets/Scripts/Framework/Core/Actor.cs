using System.Collections.Generic;
using Framework.Core.Modules;
using Framework.Types;
using UnityEngine;

namespace Framework.Core
{
    public class Actor : MonoBehaviour
    {
        protected virtual void Awake()
        {
            _steppable = new List<IStepable>();
        }

        protected virtual void Update()
        {
            foreach (var item in _steppable)
            {
                item.Step(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            
            foreach (var item in _steppable)
            {
                item.PhysicsStep(Time.fixedDeltaTime);
            }
        }

        protected virtual void LateUpdate()
        {
            foreach (var item in _steppable)
            {
                item.PostStep(Time.deltaTime);
            }

            foreach (var item in _steppable)
            {
                item.PreStep(Time.deltaTime);
            }
        }

        private List<IStepable> _steppable;

        protected void AddSteppable(IStepable steppable)
        {
            _steppable.Add(steppable);
        }

        protected void RemoveSteppable(IStepable steppable)
        {
            _steppable.Remove(steppable);
        }

        //Todo move to a more appropriate lcoation
        protected IInitializable<T> GetFrameworkComponent<T>()
        {
            return this.GetComponent<IInitializable<T>>();
        }
    }
}