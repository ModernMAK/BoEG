using System;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Ability
{
    [Obsolete("Use IReflectableAbility")]
    public interface IGroundTargetAbility : IAbility
    {
        void CastGroundTarget(Vector3 worldPos);
    }
    public interface IReflectableAbility
	{
        void CastReflected(Actor caster);
	}
}