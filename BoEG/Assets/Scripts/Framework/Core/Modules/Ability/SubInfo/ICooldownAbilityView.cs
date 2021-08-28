using System;

namespace MobaGame.Framework.Core.Modules.Ability
{
    [Obsolete("Use ICooldownView instead")]
    public interface ICooldownAbilityView : IView
    {
        float Cooldown { get; }
        float CooldownRemaining { get; }
        float CooldownNormal { get; }
    }
}