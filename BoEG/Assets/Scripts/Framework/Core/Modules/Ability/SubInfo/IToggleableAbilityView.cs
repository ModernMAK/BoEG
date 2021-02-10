using System;

namespace MobaGame.Framework.Core.Modules.Ability
{
    public interface IToggleableAbilityView : IView
    {
        bool Active { get; }
        bool ShowActive { get; }
        event EventHandler<ChangedEventArgs<bool>> Toggled;
    }
}