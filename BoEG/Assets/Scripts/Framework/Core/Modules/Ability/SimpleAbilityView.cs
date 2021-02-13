using System;
using MobaGame.Entity.Abilities;
using MobaGame.Framework.Core.Modules.Ability.Helpers;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Ability
{
    public class SimpleAbilityView : IAbilityView
    {

        private Sprite _icon;
        public Sprite Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                OnChanged();
            } 
        }


        private ICooldownView _cooldown;
        public ICooldownView Cooldown
        {
            get => _cooldown;
            set
            {
                if (_cooldown != null)
                    _cooldown.Changed -= OnInternalChanged;
                _cooldown = value;
                if (_cooldown != null)
                    _cooldown.Changed += OnInternalChanged;
            }
        }

        private void OnInternalChanged(object sender, EventArgs e) => OnChanged();


        private MagicCost _statCost;
        public MagicCost StatCost { 
            get => _statCost;
            set
            {
                if (_statCost != null)
                    _statCost.Changed -= OnInternalChanged;
                _statCost = value;
                if (_statCost != null)
                    _statCost.Changed += OnInternalChanged;
            } 
        }
        IStatCostAbilityView IAbilityView.StatCost => StatCost;


        private ToggleableAbilityView _toggleable;
        public ToggleableAbilityView Toggleable { 
            get => _toggleable;
            set
            {
                if (_toggleable != null)
                    _toggleable.Changed -= OnInternalChanged;
                _toggleable = value;
                if (_toggleable != null)
                    _toggleable.Changed += OnInternalChanged;
            }
            
        }
        IToggleableAbilityView IAbilityView.Toggleable => Toggleable;

        public event EventHandler Changed;
        public void OnChanged() => Changed?.Invoke(this,EventArgs.Empty);
    }
}