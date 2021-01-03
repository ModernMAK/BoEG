using System;
using UnityEngine;

namespace Framework.Ability
{
    public interface IAbilityView
    {
        Sprite GetIcon();


        ICooldownAbility Cooldown { get; }
        IStatCostAbility StatCost { get; }

        IToggleableAbility Toggleable { get; }

    }
}