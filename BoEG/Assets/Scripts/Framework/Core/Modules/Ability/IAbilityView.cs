using UnityEngine;

namespace Framework.Ability
{
    public interface IAbilityView
    {
        Sprite GetIcon();
        float GetCooldownProgress();
        float GetManaCost();
    }
}