using UnityEngine;

namespace Old.Entity.Modules.Abilityable
{
    public interface IAbility
    {
        void Initialize(GameObject go);
        void Trigger();
    }
}