using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Ability
{
    public interface IVectorTargetAbility : IAbility
    {
        void CastVectorTarget(Vector3 worldPos, Vector3 direction);
    }
}