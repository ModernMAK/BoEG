namespace MobaGame.Framework.Core.Modules.Ability
{
    public interface IStatCostAbilityView 
    {
        float Cost { get; }
        bool CanSpendCost();

    }
}