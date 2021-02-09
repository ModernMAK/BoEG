namespace MobaGame.Framework.Core.Modules.Ability
{
    public class StatCostAbilityView : IStatCostAbilityView
    {
        public StatCostAbilityView(IMagicable magicable, float cost = default)
        {
            _magicable = magicable;
            Cost = cost;
        }

        private readonly IMagicable _magicable;
        public float Cost { get; set; }
        public bool CanSpendCost() => _magicable.HasMagic(Cost);
    }
}