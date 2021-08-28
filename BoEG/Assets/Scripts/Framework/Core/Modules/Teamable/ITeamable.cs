using System;
using System.Collections.Generic;

namespace MobaGame.Framework.Core.Modules
{
    public interface ITeamable
    {
        /// <summary>
        /// Returns a reference to our team.
        /// Contains extra information, such as team color.
        /// </summary>
        TeamData Team { get; }
        /// <summary>
        /// Called after our team has changed.
        /// </summary>
        event EventHandler<ChangedEventArgs<TeamData>> TeamChanged; 
        /// <summary>
        /// Sets the team to a new value.
        /// </summary>
        /// <param name="team"></param>
        /// <remarks>
        /// This should only be used for effects which change a unit's team. E.G. a convert spell.
        /// </remarks>
        void SetTeam(TeamData team);
        /// <summary>
        /// Gets the relation between this teamable, and the other teamable.
        /// </summary>
        /// <param name="other">The other teamable.</param>
        /// <returns>The relation between the two; Ally, Neutral, or Enemy.</returns>
        /// <remarks>This is an asymetric property. Use </remarks>
        TeamRelation GetRelation(ITeamable other);
    }
    public static class ITeamableX
    {
        
        public static bool SameTeam(this ITeamable teamable, ITeamable other, bool defaultResult = false) => teamable == other || ((teamable != null && other != null) ? teamable.Team == other.Team : defaultResult);
        /// <summary>
        /// Check if the provided relation is the relation from other to teamable.
        /// </summary>
        /// <param name="teamable">This teamable.</param>
        /// <param name="other">The other teamable.</param>
        /// <param name="relation">The relation to check.</param>
        /// <returns>True if the relation between the two is the same as the one provided.</returns>
        public static bool IsRelation(this ITeamable teamable, ITeamable other, TeamRelation relation) => teamable.GetRelation(other) == relation;
        /// <summary>
        /// Checks If teamable and other have the same relation.
        /// </summary>
        /// <param name="teamable">This teamable.</param>
        /// <param name="other">The other teamable.</param>
        /// <returns>True if they have the same relation.</returns>
        public static bool IsRelationSymetric(this ITeamable teamable, ITeamable other) => teamable.GetRelation(other) == other.GetRelation(teamable);
        /// <summary>
        /// Check if other is an ally of teamable.
        /// </summary>
        /// <param name="teamable">This teamable.</param>
        /// <param name="other">The other teamable.</param>
        /// <returns>True if other is an ally of teamable.</returns>
        public static bool IsAlly(this ITeamable teamable, ITeamable other) => teamable.IsRelation(other, TeamRelation.Ally);
        /// <summary>
        /// Check if other and teamable are both allies.
        /// </summary>
        /// <param name="teamable">This teamable.</param>
        /// <param name="other">The other teamable.</param>
        /// <returns>True if both other and teamable are allies.</returns>
        public static bool IsAllySymetric(this ITeamable teamable, ITeamable other) => teamable.IsAlly(other) && teamable.IsRelationSymetric(other);
        /// <summary>
        /// Check if other is an enemy of teamable.
        /// </summary>
        /// <param name="teamable">This teamable.</param>
        /// <param name="other">The other teamable.</param>
        /// <returns>True if other is an enemy of teamable.</returns>
        public static bool IsEnemy(this ITeamable teamable, ITeamable other) => teamable.IsRelation(other, TeamRelation.Enemy);
        /// <summary>
        /// Check if other and teamable are both enemies.
        /// </summary>
        /// <param name="teamable">This teamable.</param>
        /// <param name="other">The other teamable.</param>
        /// <returns>True if both other and teamable are enemies.</returns>
        public static bool IsEnemySymetric(this ITeamable teamable, ITeamable other) => teamable.IsEnemy(other) && teamable.IsRelationSymetric(other);
        /// <summary>
        /// Check if other is neutral to teamable.
        /// </summary>
        /// <param name="teamable">This teamable.</param>
        /// <param name="other">The other teamable.</param>
        /// <returns>True if other is neutral to teamable.</returns>
        public static bool IsNeutral(this ITeamable teamable, ITeamable other) => teamable.IsRelation(other, TeamRelation.Neutral);
        /// <summary>
        /// Check if other and teamable are both neutral.
        /// </summary>
        /// <param name="teamable">This teamable.</param>
        /// <param name="other">The other teamable.</param>
        /// <returns>True if both other and teamable are neutral.</returns>
        public static bool IsNeutralSymetric(this ITeamable teamable, ITeamable other) => teamable.IsNeutral(other) && teamable.IsRelationSymetric(other);

    }

    /// <summary>
    /// A relation between teams.
    /// </summary>
    public enum TeamRelation : byte
	{
        Ally,
        Enemy,
        Neutral
	}

    [Flags]
    public enum TeamRelationFlag : byte
    {
        Ally = (1 << TeamRelation.Ally),
        Enemy = (1 << TeamRelation.Enemy),
        Neutral = (1 << TeamRelation.Neutral),
    }

    public static class TeamRelationX
    {
        public static TeamRelationFlag ToFlag(this TeamRelation relation) => (TeamRelationFlag)(1 << (int) relation);

        public static TeamRelationFlag ToFlags(params TeamRelation[] relations)
        {
            TeamRelationFlag flags = 0;
            foreach (var relation in relations)
                flags |= relation.ToFlag();
            return flags;
        }

        public static bool HasValue(this TeamRelationFlag flags, TeamRelation value)
        {
            var valueAsFlag = value.ToFlag();
            return flags.HasFlag(valueAsFlag);
        }
        
        public static IEnumerable<TeamRelation> ToValues(this TeamRelationFlag relationFlags)
        {
            const int teamRelationCount = 3;
            for (var i = 0; i < teamRelationCount; i++)
            {
                var flag = (TeamRelationFlag)(1 << i);
                var value = (TeamRelation)i;
                if (relationFlags.HasFlag(flag))
                    yield return value;
            }
        }
    }
}