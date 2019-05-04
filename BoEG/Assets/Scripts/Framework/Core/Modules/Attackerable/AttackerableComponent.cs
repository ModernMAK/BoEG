using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class AttackerableComponent : MonoBehaviour, IAttackerable, IComponent<IAttackerable>
    {
        private IAttackerable _attackerable;
        public float AttackDamage => _attackerable.AttackDamage;

        public float AttackRange => _attackerable.AttackRange;

        public float AttackSpeed => _attackerable.AttackSpeed;

        public float AttackInterval => _attackerable.AttackInterval;

        public float AttackCooldown => _attackerable.AttackCooldown;

        public bool IsRanged => _attackerable.IsRanged;

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