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
}