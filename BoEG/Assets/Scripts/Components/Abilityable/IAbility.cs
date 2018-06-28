using UnityEngine;

namespace Components.Abilityable
{
    public interface IAbility
    {
        void Initialize(GameObject go);
        void Trigger();
    }
}