namespace MobaGame.Framework.Core.Modules.Ability
{
    public interface IToggleableAbilityView : IAbility
    {
        bool Active { get; }
    }
}