namespace Modules.Abilityable.Ability
{
    public interface IAbilityCooldown
    {
        float Cooldown { get; }

        void StartCooldown();

        void ResetCooldown();

        float CooldownRemaining { get; }
        float CooldownRemainingNormalized { get; }

        bool OffCooldown { get; }
    }
}