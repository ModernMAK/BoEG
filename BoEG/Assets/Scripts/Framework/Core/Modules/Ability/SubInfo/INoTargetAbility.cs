using System;

namespace MobaGame.Framework.Core.Modules.Ability
{
    [Obsolete("Use IReflectableAbility")]
    public interface INoTargetAbility : IAbility
    {
        void CastNoTarget();
    }
}