using Framework.Types;
using UnityEngine;

namespace Framework.Core.Trigger
{
    public class Trigger : MonoBehaviour, ISpawnable, IInstantiable
    {
        void ISpawnable.Spawn()
        {
            Spawn();
        }

        protected virtual void Spawn()
        {
        }

        void ISpawnable.Despawn()
        {
            Despawn();
        }

        protected virtual void Despawn()
        {
        }

        void IInstantiable.Instantiate()
        {
            Instantiate();
        }

        protected virtual void Instantiate()
        {
        }

        void IInstantiable.Terminate()
        {
            Terminate();
        }

        protected virtual void Terminate()
        {
        }
    }

    
}