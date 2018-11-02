using Entity;
using UnityEngine;

namespace Modules.Abilityable.Ability
{
    public interface IAbility : IStepable
    {
        void Initialize(GameObject go);
        void Terminate();
        void Trigger();
        void LevelUp();
    }
}