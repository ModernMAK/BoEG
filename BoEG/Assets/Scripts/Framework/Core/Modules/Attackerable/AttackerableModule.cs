using System;
using System.Collections.Generic;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Core.Trigger;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    [DisallowMultipleComponent]
    public class AttackerableModule : MonoBehaviour, IAttackerable, IInitializable<IAttackerableData>,
        IListener<IStepableEvent>, IRespawnable
    {
        private Attackerable _attackerable;

        private void Awake()
        {
            var actor = GetComponent<Actor>();
            var teamable = GetComponent<ITeamable>();
            _attackerable = new Attackerable(actor, teamable);
        }


        public float AttackDamage => _attackerable.AttackDamage;
        public float AttackRange => _attackerable.AttackRange;
        public float AttackSpeed => _attackerable.AttackSpeed;

        public float AttackCooldown => _attackerable.AttackCooldown;

        public float AttackInterval => _attackerable.AttackInterval;

        public bool IsRanged => _attackerable.IsRanged;

        public bool IsAttackOnCooldown => _attackerable.IsAttackOnCooldown;


        public void Attack(Actor actor) => _attackerable.Attack(actor);

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


        public bool HasAttackTarget() => _attackerable.HasAttackTarget();

        public IReadOnlyList<Actor> GetAttackTargets() => _attackerable.GetAttackTargets();

        public Actor GetAttackTarget(int index) => _attackerable.GetAttackTarget(index);

        public int AttackTargetCounts => _attackerable.AttackTargetCounts;


        private void OnDrawGizmos()
        {
            var color = Gizmos.color; //Do we still need to do this?
            Gizmos.color = Color.Lerp(Color.black, Color.red, 0.5f);
            Gizmos.DrawWireSphere(transform.position, AttackRange);
            Gizmos.color = color;
        }

        private void OnDrawGizmosSelected()
        {
            var color = Gizmos.color; //Do we still need to do this?
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
            Gizmos.color = color;
        }

        public void Initialize(IAttackerableData module) => _attackerable.Initialize(module);


        public void Register(IStepableEvent source) => _attackerable.Register(source);

        public void Unregister(IStepableEvent source) => _attackerable.Unregister(source);

        public void Respawn() => _attackerable.Respawn();
    }
}