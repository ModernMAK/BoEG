namespace Framework.Ability
{
    public interface IToggleableAbility : IAbility
    {
        bool Active { get; }
    }
}