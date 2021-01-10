using System;
using System.Collections.Generic;
using Framework.Core;
using MobaGame.Framework.Core.Trigger;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public class Attackerable : ActorModule, IAttackerable, IInitializable<IAttackerableData>,
        IListener<IStepableEvent>, IRespawnable
    {
        public const string AttackerableTrigger = "Attackerable Trigger";
        private readonly AttackTargetTrigger<SphereCollider> _trigger;
        private IReadOnlyList<Actor> Targets => _trigger.Targets;

        private readonly DurationTimer _attackCooldown;
        private readonly ITeamable _teamable;

        public Attackerable(Actor actor, ITeamable teamable = default) : base(actor)
        {
            _teamable = teamable;

            var helper = TriggerUtility.CreateTrigger<SphereCollider>(Actor, AttackerableTrigger);
            _trigger = new AttackTargetTrigger<SphereCollider>(Actor, helper, _teamable);

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

            if (_teamable != null && actor.TryGetModule<ITeamable>(out var teamable))
                if (_teamable.SameTeam(teamable))
                    return;

            if (!actor.TryGetModule<IDamageTarget>(out var damageTarget))
                return;

            if (!actor.TryGetModule<ITargetable>(out var targetable))
            {
                var dmg = GetAttackDamage();
                var args = new AttackTargetEventArgs(Actor, dmg);
                var callback = GetAttackCallback(actor, damageTarget);
                targetable.TargetAttack(callback, args);
            }
            else
            {
                InternalPerformAttack(actor, damageTarget);
            }
        }

        private Damage GetAttackDamage() => new Damage(AttackDamage, DamageType.Physical, DamageModifiers.Attack);

        private Action GetAttackCallback(Actor actor, IDamageTarget damageTarget)
        {
            Action<Actor, IDamageTarget> func = InternalPerformAttack;
            return func.Partial(actor).Partial(damageTarget);
        }

        private void InternalPerformAttack(Actor actor, IDamageTarget damageTarget)
        {
            var dmg = GetAttackDamage();
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