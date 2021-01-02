using UnityEngine;

namespace Framework.Ability
{
    public interface IGroundTargetAbility : IAbility
    {
        void CastGroundTarget(Vector3 worldPos);
    }
}