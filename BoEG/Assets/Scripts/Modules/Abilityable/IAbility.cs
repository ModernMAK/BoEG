using Entity;
using UnityEngine;

namespace Modules.Abilityable
{
    public interface IAbility : IStepable
    {
        void Initialize(GameObject go);
        void Terminate();
        void Trigger();
        void LevelUp();
    }
}