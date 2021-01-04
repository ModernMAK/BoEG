namespace MobaGame.Framework.Core.Modules.Ability
{
    public interface IStatCostAbility : IAbility
    {
        float Cost { get; }
        bool CanSpendCost();

    }
}