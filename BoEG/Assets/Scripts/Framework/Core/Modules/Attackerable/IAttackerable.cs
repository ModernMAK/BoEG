using System;
using System.Collections.Generic;

namespace MobaGame.Framework.Core.Modules
{
    public interface IAttackerable
    {
        float AttackDamage { get; }
        float AttackRange { get; }

        float AttackSpeed { get; }

        //How fast 
        float AttackInterval { get; }

        /// <summary>
        ///     The time in seconds for another attack to be ready.
        /// </summary>
        float AttackCooldown { get; }

        bool IsRanged { get; }
        bool IsAttackOnCooldown { get; }

        void Attack(Actor actor);

        event EventHandler<AttackerableEventArgs> Attacking;

        event EventHandler<AttackerableEventArgs> Attacked;
        bool HasAttackTarget();
        IReadOnlyList<Actor> GetAttackTargets();
        Actor GetAttackTarget(int index);
        int AttackTargetCounts { get; }
    }
}