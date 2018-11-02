namespace Modules.Abilityable
{
    public interface IAbilityManacost
    {
        float ManaCost { get; }
        bool HasEnoughMana { get; }
    }
}