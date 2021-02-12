using System;
using MobaGame.Framework.Core.Modules.Ability.Helpers;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Ability
{
    public class AbilityView : IAbilityView
    {
        public Sprite Icon { get; set; }


        public ICooldownView Cooldown { get; set; }
        public IStatCostAbilityView StatCost { get; set; }

        public IToggleableAbilityView Toggleable { get; set; }
        public event EventHandler Changed;
    }
}