using UnityEngine;

namespace Framework.Ability
{
    public interface IVectorTargetAbility : IAbility
    {
        void CastVectorTarget(Vector3 worldPos, Vector3 direction);
    }
}