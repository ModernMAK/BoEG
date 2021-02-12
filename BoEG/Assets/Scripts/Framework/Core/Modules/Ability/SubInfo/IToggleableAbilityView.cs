using System;

namespace MobaGame.Framework.Core.Modules.Ability
{
    public interface IToggleable
    {
        bool Active { get; }
        event EventHandler<ChangedEventArgs<bool>> Toggled;
    }
    public interface IToggleableAbilityView : IToggleable, IView
    {
        bool ShowActive { get; }
    }
}