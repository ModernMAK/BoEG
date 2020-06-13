using System;

namespace Framework.Core.Modules
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
    }
}