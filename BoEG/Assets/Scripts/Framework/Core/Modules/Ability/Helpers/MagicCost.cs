using System;
using MobaGame.Framework.Utility;

namespace MobaGame.Framework.Core.Modules.Ability.Helpers
{
    public class MagicCost : IStatCostAbilityView
    {
        public MagicCost(IMagicable magicable, float cost = default)
        {
            _magicable = magicable;
            Cost = cost;
        }

        private readonly IMagicable _magicable;
        private float _cost;
        public float Cost
        {
            get => _cost;
            set
            {
                var changed = !_cost.SafeEquals(value);
                _cost = value;
                if (changed)
                    OnChanged();
            }
        }
        public bool CanSpendCost() => _magicable.HasMagic(Cost);
        public bool TrySpendCost() => _magicable.TrySpendMagic(Cost);
        public void SpendCost() => _magicable.SpendMagic(Cost);
        public event EventHandler Changed;
        private void OnChanged() => Changed?.Invoke(this,EventArgs.Empty);
    }
}