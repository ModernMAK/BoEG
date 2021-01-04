using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Ability
{
    public interface IGroundTargetAbility : IAbility
    {
        void CastGroundTarget(Vector3 worldPos);
    }
}