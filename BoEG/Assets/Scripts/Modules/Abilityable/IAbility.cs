using System;
using Entity;
using UnityEngine;

namespace Modules.Abilityable
{
    [Obsolete]
    public interface IAbility : IStepableOld
    {
        void Initialize(GameObject go);
        void Terminate();
        void Trigger();
        void LevelUp();
    }
}