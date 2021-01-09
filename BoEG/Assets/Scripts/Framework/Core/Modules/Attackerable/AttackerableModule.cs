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
        private Actor _actor;

        public const string AttackerableTrigger = "Attackerable Trigger";
        private TargetTrigger<SphereCollider> _trigger;
        private IReadOnlyList<Actor> Targets => _trigger.Targets;

        private DurationTimer _attackCooldown;
        private ITeamable _teamable;

        private void Awake()
        {
            _actor = GetComponent<Actor>();
            _teamable = GetComponent<ITeamable>();
            var helper = TriggerUtility.CreateTrigger<SphereCollider>(_actor, AttackerableTrigger);
            _trigger = new TargetTrigger<SphereCollider>(_actor, helper, _teamable);


            _attackCooldown = new DurationTimer(0f);
            _attackCooldown.SetDone();
        }



        public float AttackDamage { get; protected set; }
        public float AttackRange { get; protected set; }
        public float AttackSpeed { get; protected set; }

        public float AttackCooldown => AttackInterval / AttackSpeed;

        public float AttackInterval { get; protected set; }

        public bool IsRanged { get; protected set; }

        public bool IsAttackOnCooldown => !_attackCooldown.Done;


        private void PutAttackOnCooldown()
        {
            _attackCooldown.Duration = AttackCooldown;
            _attackCooldown.Reset();
        }

        public void Attack(Actor actor)
        {
            if (IsAttackOnCooldown)
                return;

            if (_teamable != null && actor.TryGetComponent<ITeamable>(out var teamable))
                if (_teamable.SameTeam(teamable))
                    return;

            if (!actor.TryGetComponent<IDamageTarget>(out var damageTarget))
                return;

            var dmg = new Damage(AttackDamage, DamageType.Physical, DamageModifiers.Attack);

            var attackArgs = new AttackerableEventArgs();
            OnAttacking(attackArgs);
            damageTarget.TakeDamage(gameObject, dmg);
            PutAttackOnCooldown();
            OnAttacked(attackArgs);
        }

        public event EventHandler<AttackerableEventArgs> Attacking;

        public event EventHandler<AttackerableEventArgs> Attacked;


        public bool HasAttackTarget() => Targets.Count > 0;

        public IReadOnlyList<Actor> GetAttackTargets() => Targets;

        public Actor GetAttackTarget(int index) => Targets[index];

        public int AttackTargetCounts => Targets.Count;

        protected void OnAttacking(AttackerableEventArgs e)
        {
            Attacking?.Invoke(this, e);
        }

        protected void OnAttacked(AttackerableEventArgs e)
        {
            Attacked?.Invoke(this, e);
        }

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

        public void Initialize(IAttackerableData module)
        {
            AttackDamage = module.AttackDamage;
            AttackRange = module.AttackRange;
            AttackSpeed = module.AttackSpeed;
            IsRanged = module.IsRanged;
            AttackInterval = module.AttackInterval;
        }

        private void OnPreStep(float deltaTime)
        {
            if (!_attackCooldown.Done)
                _attackCooldown.AdvanceTime(deltaTime);
        }

        private void OnPhysicsStep(float deltaTime)
        {
            _trigger.Trigger.Collider.radius = AttackRange;
        }


        public void Register(IStepableEvent source)
        {
            source.PreStep += OnPreStep;
            source.PhysicsStep += OnPhysicsStep;
        }

        public void Unregister(IStepableEvent source)
        {
            source.PreStep -= OnPreStep;
            source.PhysicsStep -= OnPhysicsStep;
        }

        public void Respawn()
        {
            _attackCooldown.SetDone();
        }
    }
    public class Attackerable: ActorModule, IAttackerable, IInitializable<IAttackerableData>,
        IListener<IStepableEvent>, IRespawnable
    {

        public const string AttackerableTrigger = "Attackerable Trigger";
        private TargetTrigger<SphereCollider> _trigger;
        private IReadOnlyList<Actor> Targets => _trigger.Targets;

        private DurationTimer _attackCooldown;
        private ITeamable _teamable;

        private Attackerable(Actor actor, Teamable teamable = default) : base(actor)
        {
            _teamable = teamable;
            var helper = TriggerUtility.CreateTrigger<SphereCollider>(Actor, AttackerableTrigger);
            _trigger = new TargetTrigger<SphereCollider>(Actor, helper, _teamable);


            _attackCooldown = new DurationTimer(0f);
            _attackCooldown.SetDone();
        }



        public float AttackDamage { get; protected set; }
        public float AttackRange { get; protected set; }
        public float AttackSpeed { get; protected set; }

        public float AttackCooldown => AttackInterval / AttackSpeed;

        public float AttackInterval { get; protected set; }

        public bool IsRanged { get; protected set; }

        public bool IsAttackOnCooldown => !_attackCooldown.Done;


        private void PutAttackOnCooldown()
        {
            _attackCooldown.Duration = AttackCooldown;
            _attackCooldown.Reset();
        }

        public void Attack(Actor actor)
        {
            if (IsAttackOnCooldown)
                return;

            if (_teamable != null && actor.TryGetComponent<ITeamable>(out var teamable))
                if (_teamable.SameTeam(teamable))
                    return;

            if (!actor.TryGetComponent<IDamageTarget>(out var damageTarget))
                return;

            var dmg = new Damage(AttackDamage, DamageType.Physical, DamageModifiers.Attack);

            var attackArgs = new AttackerableEventArgs();
            OnAttacking(attackArgs);
            damageTarget.TakeDamage(GameObject, dmg);
            PutAttackOnCooldown();
            OnAttacked(attackArgs);
        }

        public event EventHandler<AttackerableEventArgs> Attacking;

        public event EventHandler<AttackerableEventArgs> Attacked;


        public bool HasAttackTarget() => Targets.Count > 0;

        public IReadOnlyList<Actor> GetAttackTargets() => Targets;

        public Actor GetAttackTarget(int index) => Targets[index];

        public int AttackTargetCounts => Targets.Count;

        protected void OnAttacking(AttackerableEventArgs e)
        {
            Attacking?.Invoke(this, e);
        }

        protected void OnAttacked(AttackerableEventArgs e)
        {
            Attacked?.Invoke(this, e);
        }
        
        public void Initialize(IAttackerableData module)
        {
            AttackDamage = module.AttackDamage;
            AttackRange = module.AttackRange;
            AttackSpeed = module.AttackSpeed;
            IsRanged = module.IsRanged;
            AttackInterval = module.AttackInterval;
        }

        private void OnPreStep(float deltaTime)
        {
            if (!_attackCooldown.Done)
                _attackCooldown.AdvanceTime(deltaTime);
        }

        private void OnPhysicsStep(float deltaTime)
        {
            _trigger.Trigger.Collider.radius = AttackRange;
        }


        public void Register(IStepableEvent source)
        {
            source.PreStep += OnPreStep;
            source.PhysicsStep += OnPhysicsStep;
        }

        public void Unregister(IStepableEvent source)
        {
            source.PreStep -= OnPreStep;
            source.PhysicsStep -= OnPhysicsStep;
        }

        public void Respawn()
        {
            _attackCooldown.SetDone();
        }
    }
}