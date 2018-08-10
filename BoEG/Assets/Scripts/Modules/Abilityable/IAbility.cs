using Entity;
using UnityEngine;

namespace Modules.Abilityable
{
    public interface IAbility : ITickable
    {
        void Initialize(GameObject go);
        void Terminate();
        void Trigger();
    }
}