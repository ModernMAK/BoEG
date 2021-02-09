using MobaGame.Entity.Abilities;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Ability
{
    public interface IAbilityView
    {
        Sprite Icon { get; }


        ICooldownAbilityView Cooldown { get; }
        IStatCostAbilityView StatCost { get; }

        IToggleableAbilityView Toggleable { get; }

    }

    public class AbilityView : IAbilityView
    {
        public Sprite Icon { get; set; }


        public ICooldownAbilityView Cooldown { get; set; }
        public IStatCostAbilityView StatCost { get; set; }

        public IToggleableAbilityView Toggleable { get; set; }
    }

    public class SimpleAbilityView : IAbilityView
    {
        
        public Sprite Icon { get; set; }


        public ICooldownAbilityView Cooldown { get; set; }
        public StatCostAbilityView StatCost { get; set; }
        IStatCostAbilityView IAbilityView.StatCost => StatCost;

        public ToggleableAbilityView Toggleable { get; set; }
        IToggleableAbilityView IAbilityView.Toggleable => Toggleable;
        
    }
}