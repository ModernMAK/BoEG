using MobaGame.Entity.Abilities;
using UnityEngine;

using System;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;

namespace MobaGame.Framework.Core.Modules.Ability
{
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
namespace MobaGame.Entity.Abilities
{

    public class ToggleableAbilityView : IToggleableAbilityView
    {
        public ToggleableAbilityView(object sender, bool active = default)
        {
            _sender = sender;
            _active = active;
        }

        private readonly object _sender;
        private bool _active;

        public bool Active
        {
            get => _active;
            set
            {
                var prev = _active;
                var changed = prev != value;
                _active = value;
                if(changed)
                    OnToggled(new ChangedEventArgs<bool>(prev,value));
				
            }
        }

        public event EventHandler<ChangedEventArgs<bool>> Toggled;
        private void OnToggled(ChangedEventArgs<bool> e) => Toggled?.Invoke(_sender, e);
    }

}