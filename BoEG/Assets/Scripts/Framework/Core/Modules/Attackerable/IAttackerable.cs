using MobaGame.Framework.Types;
using System;
using System.Collections.Generic;

namespace MobaGame.Framework.Core.Modules
{
    public interface IAttackerable
    {
        /// <summary>
        /// The damage value.
        /// </summary>
        IModifiedValue<float> Damage { get; }
        /// <summary>
        /// The range where attacks are allowed. 
        /// </summary>
        IModifiedValue<float> Range { get; }
        /// <summary>
        /// The number of attacks to perfrom in a single Interval.
        /// </summary>
        IModifiedValue<float> AttacksPerInterval { get; }

        /// <summary>
        /// The duration of a single attack interval.
        /// </summary>
        IModifiedValue<float> Interval { get; }

        /// <summary>
        ///     The time in seconds for 
        /// </summary>
        /// <remarks>
        /// Should always be Interval / Speed
        /// </remarks>
        float Cooldown { get; }
        /// <summary>
        /// Is the unit ranged?
        /// </summary>
        bool IsRanged { get; }
        /// <summary>
        /// Is the attack on cooldown?
        /// </summary>
        bool OnCooldown { get; }
        /// <summary>
        /// Attacks the actor, the attack will go on cooldown afterwards.
        /// </summary>
        /// <param name="actor">The actor to attack.</param>
        void Attack(Actor actor);
        /// <summary>
        /// Attacks the actor, Ignoring Cooldown, Range, and other limits.
        /// </summary>
        /// <param name="actor">The actor to attack.</param>
        /// <remarks>
        /// This should only be used in abilities.
        /// </remarks>
        void RawAttack(Actor actor, Damage damage);


        event EventHandler<AttackerableEventArgs> Attacking;

        event EventHandler<AttackerableEventArgs> Attacked;
        IReadOnlyList<Actor> Targets { get; }
    }
    public static class IAttackerableX
    {
        public static bool HasTarget(this IAttackerable attackerable) => attackerable.Targets.Count > 0;
    }
}