namespace Framework.Ability
{
    public interface IStatCostAbility : IAbility
    {
        
    }
    public interface ICooldownAbility : IAbility
    {
        float Cooldown { get; }
        float CooldownRemaining { get; }
        float CooldownNormal { get; }
    }
}