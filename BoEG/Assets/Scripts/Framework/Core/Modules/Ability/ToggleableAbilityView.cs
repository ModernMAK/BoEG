using System;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;

namespace MobaGame.Entity.Abilities
{
    public class AbilityTargetingChecker
	{
        public AbilitySelfChecker SelfCheck { get; set; }
        public AbilityTeamChecker TeamCheck { get; set; }

        public bool IsForbidden(Actor actor) => !IsAllowed(actor);
        public bool IsAllowed(Actor actor)
		{
            if (SelfCheck!=null)
                if (!SelfCheck.IsAllowed(actor))
                    return false;
            if (TeamCheck != null)
                if (!TeamCheck.IsAllowed(actor))
                    return false;
            return true;
		}
	}
    public class AbilitySelfChecker
	{
        public AbilitySelfChecker(Actor self, bool allowSelf = default)
		{
            Self = self;
            AllowSelf = allowSelf;
		}
        public Actor Self { get; }
        public bool AllowSelf { get; set; }

        public bool IsAllowed(Actor actor) => AllowSelf || actor != Self;
        public bool IsForbidden(Actor actor) => !IsAllowed(actor);

    }
    public class AbilityTeamChecker
    {
        public static AbilityTeamChecker AllyOnly(ITeamable teamable) => new AbilityTeamChecker(teamable,TeamRelationFlag.Ally);
        public static AbilityTeamChecker NonAllyOnly(ITeamable teamable) => new AbilityTeamChecker(teamable,TeamRelationFlag.Enemy | TeamRelationFlag.Neutral);
        public AbilityTeamChecker(ITeamable teamable, TeamRelationFlag allowed = default)
        {
            _teamable = teamable;
            Allowed = allowed;
        }
        private readonly ITeamable _teamable;
        public TeamRelationFlag Allowed { get; set; }

        public bool IsAllowed(Actor actor, bool defaultAllowed = false)
        {
            if (actor.TryGetModule<ITeamable>(out var otherTeamable))
                return IsAllowed(otherTeamable);
            return defaultAllowed;
        }
        public bool IsAllowed(ITeamable otherTeamable)
        {
            var relation = _teamable.GetRelation(otherTeamable);
            return IsAllowed(relation);
        }
        public bool IsAllowed(TeamRelation relation)
        {
            return Allowed.HasFlag(relation);
        }
    }

    public class ToggleableAbilityView : IToggleableAbilityView
    {
        public ToggleableAbilityView(object sender, bool active = default)
        {
            _sender = sender;
            _active = active;
        }

        private readonly object _sender;
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
        private void OnToggled(ChangedEventArgs<bool> e) => Toggled?.Invoke(_sender, e);

        public void Toggle() => Active = !Active;
        public event EventHandler Changed;
    }

}