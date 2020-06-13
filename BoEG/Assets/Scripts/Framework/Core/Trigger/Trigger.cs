using Framework.Types;
using UnityEngine;

namespace Framework.Core.Trigger
{
    public class Trigger : MonoBehaviour, ISpawnable, IInstantiable
    {
        void IInstantiable.Instantiate()
        {
            Instantiate();
        }

        void IInstantiable.Terminate()
        {
            Terminate();
        }

        void ISpawnable.Spawn()
        {
            Spawn();
        }

        void ISpawnable.Despawn()
        {
            Despawn();
        }

        protected virtual void Spawn()
        {
        }

        protected virtual void Despawn()
        {
        }

        protected virtual void Instantiate()
        {
        }

        protected virtual void Terminate()
        {
        }
    }
}