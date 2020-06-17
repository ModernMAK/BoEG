using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core.Modules
{
    [AddComponentMenu("EndGame/Components/Attackerable")]
    [DisallowMultipleComponent]
    public class Attackerable : MonoBehaviour, IAttackerable, IInitializable<IAttackerableData>
    {
        private readonly float _cooldowEnd = float.MinValue;
        

        public float AttackDamage { get; protected set; }
        public float AttackRange { get; protected set; }
        public float AttackSpeed { get; protected set; }

        public float AttackCooldown => AttackInterval / AttackSpeed;

        public float AttackInterval { get; protected set; }

        public bool IsRanged { get; protected set; }

        public bool IsAttackOnCooldown => Time.time <= _cooldowEnd;


        public void Attack(Actor actor)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<AttackerableEventArgs> Attacking;

        public event EventHandler<AttackerableEventArgs> Attacked;
        
        
        public bool HasAttackTarget()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Actor> GetAttackTargets()
        {
            throw new NotImplementedException();
        }

        public Actor GetAttackTarget(int index)
        {
            throw new NotImplementedException();
        }

        public int AttackTargetCounts => throw new NotImplementedException();

        protected void OnAttacking(AttackerableEventArgs e)
        {
            Attacking?.Invoke(this, e);
        }

        protected void OnAttacked(AttackerableEventArgs e)
        {
            Attacked?.Invoke(this, e);
        }

        private void OnDrawGizmosSelected()
        {
            var color = Gizmos.color; //Do we still need to do this?
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
            Gizmos.color = color;
        }

        public void Initialize(IAttackerableData module)
        {
            AttackDamage = module.AttackDamage;
            AttackRange = module.AttackRange;
            AttackSpeed = module.AttackSpeed;
            IsRanged = module.IsRanged;
        }
    }
}