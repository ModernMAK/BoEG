using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    [AddComponentMenu("EndGame/Components/Attackerable")]
    [DisallowMultipleComponent]
    public class AttackerableComponent : MonoBehaviour, IAttackerable, IInitializable<IAttackerable>
    {
        private IAttackerable _attackerable;
        public float AttackDamage => _attackerable.AttackDamage;

        public float AttackRange => _attackerable.AttackRange;

        public float AttackSpeed => _attackerable.AttackSpeed;

        public float AttackInterval => _attackerable.AttackInterval;

        public float AttackCooldown => _attackerable.AttackCooldown;

        public bool IsRanged => _attackerable.IsRanged;

        public bool IsAttackOnCooldown => _attackerable.IsAttackOnCooldown;

        private void OnDrawGizmosSelected()
        {
            if (_attackerable == null)
                return;
            
            var color = Gizmos.color; //Do we still need to do this?
            Gizmos.color = Color.red;            
            Gizmos.DrawWireSphere(transform.position,AttackRange);
            Gizmos.color = color;
        }

        public void Attack(Actor actor)
        {
            _attackerable.Attack(actor);
        }

        public event EventHandler<AttackerableEventArgs> Attacking
        {
            add => _attackerable.Attacking += value;
            remove => _attackerable.Attacking -= value;
        }

        public event EventHandler<AttackerableEventArgs> Attacked
        {
            add => _attackerable.Attacked += value;
            remove => _attackerable.Attacked -= value;
        }

        public void Initialize(IAttackerable module)
        {
            _attackerable = module;
        }
    }
}