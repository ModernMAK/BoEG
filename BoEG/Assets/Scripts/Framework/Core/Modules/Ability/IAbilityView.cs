using System;
using UnityEngine;

namespace Framework.Ability
{
    public interface IAbilityView
    {
        Sprite GetIcon();

        [Obsolete("Use Cooldown")]
        float GetCooldownProgress();

        ICooldownAbility Cooldown { get; }
        IStatCostAbility StatCost { get; }

        IToggleableAbility Toggleable { get; }

        [Obsolete("Use StatCost")]
        float GetManaCost();
    }
}