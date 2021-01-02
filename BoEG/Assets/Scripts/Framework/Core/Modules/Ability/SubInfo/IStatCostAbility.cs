namespace Framework.Ability
{
    public interface IStatCostAbility : IAbility
    {
        float Cost { get; }
        bool CanSpendCost();

    }
}