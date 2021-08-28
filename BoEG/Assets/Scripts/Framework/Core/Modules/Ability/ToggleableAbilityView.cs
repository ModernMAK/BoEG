using System;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;

namespace MobaGame.Entity.Abilities
{

    public class ToggleableAbilityView : IToggleableAbilityView
    {
        public ToggleableAbilityView(bool active = default)
        {
            _active = active;
        }

        private bool _active;

        private bool _showActive;
        public bool ShowActive
        {
            get => _showActive;
            set
            {
                _showActive = value;
                OnChanged();
                
            }
        }
        public bool Active
        {
            get => _active;
            set
            {
                var prev = _active;
                var changed = prev != value;
                _active = value;
                if (changed)
                {
                    OnToggled(new ChangedEventArgs<bool>(prev,value));
                    OnChanged();
                }
				
            }
        }

        private void OnChanged() => Changed?.Invoke(this,EventArgs.Empty);

        public event EventHandler<ChangedEventArgs<bool>> Toggled;
        private void OnToggled(ChangedEventArgs<bool> e) => Toggled?.Invoke(this, e);

        public void Toggle() => Active = !Active;
        public event EventHandler Changed;
    }

}