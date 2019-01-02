using Framework.Types;
using UnityEngine;

namespace Framework.Ability.Hero.WarpedMagi
{
    public abstract class BetterAbility : Ability, IInstantiable, ISpawnable
    {
        protected bool IsInstantiated { get; private set; }
        protected bool IsSpawned { get; private set; }

        void IInstantiable.Instantiate()
        {
            Instantiate();
            IsInstantiated = true;
        }

        protected virtual void Instantiate()
        {
        }

        protected virtual void Terminate()
        {
        }

        void IInstantiable.Terminate()
        {
            Terminate();
            IsInstantiated = false;
        }

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
    }
}