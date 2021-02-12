using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;

namespace MobaGame.Framework.Core.Modules.Ability.Helpers
{
	public class TeamableChecker
    {
        public static TeamableChecker AllyOnly(ITeamable teamable) => new TeamableChecker(teamable, TeamRelationFlag.Ally);
        public static TeamableChecker NonAllyOnly(ITeamable teamable) => new TeamableChecker(teamable, TeamRelationFlag.Enemy | TeamRelationFlag.Neutral);
        public TeamableChecker(ITeamable teamable, TeamRelationFlag allowed = default)
        {
            _teamable = teamable;
            Allowed = allowed;
        }
        private readonly ITeamable _teamable;
        public TeamRelationFlag Allowed { get; set; }

        //To use as a predicate, we can't use optional arguments
        public bool IsAllowed(Actor actor) => IsAllowed(actor, false);
        public bool IsAllowed(Actor actor, bool defaultAllowed)
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
            var flag = relation.ToFlag();
            return IsAllowed(flag);
        }

        public bool IsAllowed(TeamRelationFlag flag)
        {
            return Allowed.HasFlag(flag);
        }
    }
}