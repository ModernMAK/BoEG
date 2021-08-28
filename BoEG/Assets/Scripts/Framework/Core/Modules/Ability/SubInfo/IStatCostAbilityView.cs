namespace MobaGame.Framework.Core.Modules.Ability
{
    public interface IStatCostAbilityView : IView
    {
        float Cost { get; }
        bool CanSpendCost();

    }
}