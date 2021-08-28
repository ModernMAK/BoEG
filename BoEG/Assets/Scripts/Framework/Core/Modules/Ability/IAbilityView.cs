using MobaGame.Framework.Core.Modules.Ability.Helpers;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Ability
{
    public interface IAbilityView : IView
    {
        Sprite Icon { get; }


        ICooldownView Cooldown { get; }
        IStatCostAbilityView StatCost { get; }

        IToggleableAbilityView Toggleable { get; }

    }
}