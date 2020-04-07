using System;
using UnityEngine.Analytics;

namespace Framework.Core.Modules
{
    public class Attackerable : IAttackerable
    {
        public Attackerable(float damage, float range, float speed, float interval, bool ranged)
        {
            AttackDamage = damage;
            AttackRange = range;
            AttackSpeed = speed;
            IsRanged = ranged;
        }

        public Attackerable(IAttackerableData data) : this(data.AttackDamage, data.AttackRange, data.AttackSpeed,
            data.AttackInterval, data.IsRanged)
        {
        }

        public float AttackDamage { get; protected set; }
        public float AttackRange { get; protected set; }
        public float AttackSpeed { get; protected set; }

        public float AttackCooldown => AttackInterval / AttackSpeed;

        public float AttackInterval { get; protected set; }

        public bool IsRanged { get; protected set; }


        public void Attack(Actor actor)
        {
            throw new System.NotImplementedException();
        }

        public event EventHandler<AttackerableEventArgs> Attacking;

        protected void OnAttacking(AttackerableEventArgs e)
        {
            Attacking?.Invoke(this, e);
        }

        public event EventHandler<AttackerableEventArgs> Attacked;

        protected void OnAttacked(AttackerableEventArgs e)
        {
            Attacked?.Invoke(this, e);
        }
    }
}