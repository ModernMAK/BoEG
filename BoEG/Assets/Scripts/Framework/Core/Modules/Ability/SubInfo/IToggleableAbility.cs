namespace MobaGame.Framework.Core.Modules.Ability
{
    public interface IToggleableAbility : IAbility
    {
        bool Active { get; }
    }
}