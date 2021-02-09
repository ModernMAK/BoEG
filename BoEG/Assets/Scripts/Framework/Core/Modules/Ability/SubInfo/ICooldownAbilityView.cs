namespace MobaGame.Framework.Core.Modules.Ability
{
    public interface ICooldownAbilityView
    {
        float Cooldown { get; }
        float CooldownRemaining { get; }
        float CooldownNormal { get; }
    }
}