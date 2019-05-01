using System;
using UnityEngine.Analytics;

namespace Framework.Core.Modules
{
    
    public class Attackerable : IAttackerable
    {
        public float AttackDamage { get; protected set; }
        public float AttackRange { get; protected set; }
        public float AttackSpeed { get; protected set; }

        public float AttackCooldown
        {
            get { return AttackInterval / AttackSpeed; }
        }

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