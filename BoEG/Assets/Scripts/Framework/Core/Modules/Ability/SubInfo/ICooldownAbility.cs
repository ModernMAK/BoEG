namespace MobaGame.Framework.Core.Modules.Ability
{
    public interface ICooldownAbility : IAbility
    {
        float Cooldown { get; }
        float CooldownRemaining { get; }
        float CooldownNormal { get; }
    }
}