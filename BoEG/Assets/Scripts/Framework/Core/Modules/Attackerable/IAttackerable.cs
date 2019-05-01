using System;

namespace Framework.Core.Modules
{
    public interface IAttackerable
    {
        float AttackDamage { get; }
        float AttackRange { get; }
        float AttackSpeed { get; }
        float AttackInterval { get; }
        float AttackCooldown { get; }
        bool IsRanged { get; }

        void Attack(Actor actor);

        event EventHandler<AttackerableEventArgs> Attacking;
        
        event EventHandler<AttackerableEventArgs> Attacked;
    }
}