using System;

namespace MobaGame.Framework.Core.Modules.Ability
{
    public interface IToggleableAbilityView
    {
        bool Active { get; }
        event EventHandler<ChangedEventArgs<bool>> Toggled;
    }
}