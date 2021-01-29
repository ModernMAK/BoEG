using System;
using System.Collections.Generic;
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
        public IReadOnlyList<Actor> Targets => _trigger.Targets;

        private readonly DurationTimer _attackCooldown;
        private readonly ITeamable _teamable;
        private readonly ModifiedValue _damage;
        private readonly ModifiedValue _range;
        private readonly ModifiedValue _attacksPerInterval;
        private readonly ModifiedValue _interval;

        public Attackerable(Actor actor, ITeamable teamable = default) : base(actor)
        {
            _teamable = teamable;

            var helper = TriggerUtility.CreateTrigger<SphereCollider>(Actor, AttackerableTrigger);
            _trigger = new AttackTargetTrigger<SphereCollider>(Actor, helper, _teamable);

            _attackCooldown = new DurationTimer(0f);
            _attackCooldown.SetDone();
            _damage = new ModifiedValue();
            _range = new ModifiedValue();
            _attacksPerInterval = new ModifiedValue();
            _interval = new ModifiedValue();
        }


        public IModifiedValue<float> Damage => _damage;
        public IModifiedValue<float> Range => _range;
        public IModifiedValue<float> AttacksPerInterval => _attacksPerInterval;

        public float Cooldown => Interval.Total/ AttacksPerInterval.Total;

        public IModifiedValue<float> Interval => _interval;

        public bool IsRanged { get; protected set; }

        public bool OnCooldown => !_attackCooldown.Done;


        private void PutAttackOnCooldown()
        {
            _attackCooldown.Duration = Cooldown;
            _attackCooldown.Reset();
        }

        public void Attack(Actor actor)
        {
            if (OnCooldown)
                return;

            if (_teamable != null && actor.TryGetModule<ITeamable>(out var teamable))
                if (!_teamable.IsAlly(teamable))
                    return;

            PerformAttack(actor,GetAttackDamage(), true);
        }
        public void RawAttack(Actor actor, Damage damage)
        {
            PerformAttack(actor, damage, false);
        }
        void PerformAttack(Actor actor, Damage damage, bool useCooldown)
        {
            if (!actor.TryGetModule<IDamageTarget>(out var damageTarget))
                return;

            if (actor.TryGetModule<ITargetable>(out var targetable))
            {
                var args = new AttackTargetEventArgs(Actor, damage);
                var callback = GetAttackCallback(actor, damageTarget, damage, useCooldown);
                targetable.TargetAttack(callback, args);
            }
            else
            {
                InternalPerformAttack(actor, damageTarget,damage,useCooldown);
            }
        }

        private Damage GetAttackDamage() => new Damage(Damage.Total, DamageType.Physical, DamageModifiers.Attack);

        private Action GetAttackCallback(Actor actor, IDamageTarget damageTarget, Damage damage, bool useCooldown = true)
        {
            Action<Actor, IDamageTarget,Damage,bool> func = InternalPerformAttack;
            return func.Partial(actor, damageTarget, damage, useCooldown);
        }

        private void InternalPerformAttack(Actor actor, IDamageTarget damageTarget, Damage damage, bool useCooldown = true)
        {
            var attackArgs = new AttackerableEventArgs();
            OnAttacking(attackArgs);
            damageTarget.TakeDamage(Actor, damage);
            if(useCooldown)
                PutAttackOnCooldown();
            OnAttacked(attackArgs);
        }

        public event EventHandler<AttackerableEventArgs> Attacking;

        public event EventHandler<AttackerableEventArgs> Attacked;


        protected void OnAttacking(AttackerableEventArgs e)
        {
            Attacking?.Invoke(this, e);
        }

        protected void OnAttacked(AttackerableEventArgs e)
        {
            Attacked?.Invoke(this, e);
        }

        public void Initialize(IAttackerableData data)
        {
            _damage.Base = data.AttackDamage;
            _range.Base = data.AttackRange;
            _attacksPerInterval.Base = data.AttackSpeed;
            IsRanged = data.IsRanged;
            _interval.Base = data.AttackInterval;
        }

        private void OnPreStep(float deltaTime)
        {
            if (!_attackCooldown.Done)
                _attackCooldown.AdvanceTime(deltaTime);
        }

        private void OnPhysicsStep(float deltaTime)
        {
            _trigger.Trigger.Collider.radius = _range.Total;
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