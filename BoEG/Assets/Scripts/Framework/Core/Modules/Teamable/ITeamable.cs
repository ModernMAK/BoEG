using System;

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
        [Obsolete("Use Get Relation or ITeamableX.IsAlly")]
        /// <summary>
        /// Whether this instance considers the other teamable to be an ally.
        /// </summary>
        /// <param name="teamable">The other teamable instance.</param>
        /// <returns>True if ally, false otherwise.</returns>        
        bool SameTeam(ITeamable teamable);
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
    public enum TeamRelation
	{
        Ally,
        Enemy,
        Neutral
	}
}