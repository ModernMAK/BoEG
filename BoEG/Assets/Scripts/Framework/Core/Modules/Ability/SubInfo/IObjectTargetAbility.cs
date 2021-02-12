using System;

namespace MobaGame.Framework.Core.Modules.Ability
{
    [Obsolete("Use IReflectableAbility")]
    public interface IObjectTargetAbility<in TObject> : IAbility
    {
        void CastObjectTarget(TObject target);
    }
}