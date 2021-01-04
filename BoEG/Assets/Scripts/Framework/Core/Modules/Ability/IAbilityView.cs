using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Ability
{
    public interface IAbilityView
    {
        Sprite GetIcon();


        ICooldownAbility Cooldown { get; }
        IStatCostAbility StatCost { get; }

        IToggleableAbility Toggleable { get; }

    }
}