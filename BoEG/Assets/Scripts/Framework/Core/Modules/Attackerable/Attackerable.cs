using System;
using System.Collections.Generic;
using MobaGame.Framework.Core.Trigger;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public interface IAttackDamageModifier : IModifier { Modifier AttackDamage { get; } }
    public interface IAttackRangeModifier : IModifier { Modifier AttackRange { get; } }
    public interface IAttacksPerIntervalModifier : IModifier { Modifier AttacksPerInterval { get; } }
    public interface IAttackIntervalModifier : IModifier { Modifier AttackInterval { get; } }

    public class AttackCritEventArgs : EventArgs
	{
        public AttackCritEventArgs(SourcedDamage<Actor> damage)
		{
            Damage = damage;
		}
        public SourcedDamage<Actor> Damage { get; }
        /// <remarks>
        /// A value of 0 means no modifier is aplied.
        /// .5 would be a 150% chance crit
        /// </remarks>
        public float CriticalMultiplier { get; set; }

        public SourcedDamage<Actor> FinalDamage => Damage.SetDamage(Damage.Damage.SetValue(Damage.Damage.Value * CriticalMultiplier));

	}
    public class AttackLifestealEventArgs : EventArgs
	{
        public AttackLifestealEventArgs(SourcedDamage<Actor> source)
		{
            Damage = source;
		}
        public SourcedDamage<Actor> Damage { get; }
        public float LifestealAmount { get; set; }
	}

    public class Attackerable : ActorModule, IAttackerable, IInitializable<IAttackerableData>,
        IListener<IStepableEvent>, IRespawnable, IListener<IModifiable>
    {
        public const string AttackerableTrigger = "Attackerable Trigger";
        private readonly AttackTargetTrigger<SphereCollider> _trigger;
        public IReadOnlyList<Actor> Targets => _trigger.Targets;
        private readonly DurationTimer _attackCooldown;
        private readonly ITeamable _teamable;
        private readonly ModifiedValueBoilerplate<IAttackDamageModifier> _damage;
        private readonly ModifiedValueBoilerplate<IAttackRangeModifier> _range;
        private readonly ModifiedValueBoilerplate<IAttacksPerIntervalModifier> _attacksPerInterval;
        private readonly ModifiedValueBoilerplate<IAttackIntervalModifier> _interval;

        public Attackerable(Actor actor, ITeamable teamable = default) : base(actor)
        {
            _teamable = teamable;

            var helper = TriggerUtility.CreateTrigger<SphereCollider>(Actor, AttackerableTrigger);
            _trigger = new AttackTargetTrigger<SphereCollider>(Actor, helper, _teamable);
            _attackCooldown = new DurationTimer(0f,true);
            _damage = new ModifiedValueBoilerplate<IAttackDamageModifier>(modifier=>modifier.AttackDamage);
            _range = new ModifiedValueBoilerplate<IAttackRangeModifier>(modifier => modifier.AttackRange);
            _attacksPerInterval = new ModifiedValueBoilerplate<IAttacksPerIntervalModifier>(modifier => modifier.AttacksPerInterval);
            _interval = new ModifiedValueBoilerplate<IAttackIntervalModifier>(modifier => modifier.AttackInterval); 
        }

        public float Damage => _damage.Value.Total;
        public float Range => _range.Value.Total;
        public float AttacksPerInterval => _attacksPerInterval.Value.Total;

        public float Cooldown => Interval/ AttacksPerInterval;

        public float Interval => _interval.Value.Total;

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

        private Damage GetAttackDamage() => new Damage(Damage, DamageType.Physical, DamageModifiers.Attack);

        private Action GetAttackCallback(Actor actor, IDamageTarget damageTarget, Damage damage, bool useCooldown = true)
        {
            Action<Actor, IDamageTarget,Damage,bool> func = InternalPerformAttack;
            return func.Partial(actor, damageTarget, damage, useCooldown);
        }

        private void InternalPerformAttack(Actor actor, IDamageTarget damageTarget, Damage damage, bool useCooldown = true)
        {
            var sourceDamage = new SourcedDamage<Actor>(damage, Actor);
            var attackArgs = new AttackerableEventArgs(Actor, actor, damage);
            OnAttacking(attackArgs);
            var critDamage = CalculateCritDamage(sourceDamage);
            var lifesteal = CalculateLifestealModifier(sourceDamage);
            damageTarget.TakeDamage(critDamage);
            if (Actor.TryGetModule<IHealthable>(out var healthable))
                healthable.Value += lifesteal;
            if(useCooldown)
                PutAttackOnCooldown();
            var attackedArgs = new AttackerableEventArgs(actor, Actor,critDamage.Damage);
            OnAttacked(attackedArgs);
        }

        public event EventHandler<AttackerableEventArgs> Attacking;

        public event EventHandler<AttackerableEventArgs> Attacked;

        public event EventHandler<AttackCritEventArgs> CritModifiers;
        public event EventHandler<AttackLifestealEventArgs> LifestealModifiers;


        protected float CalculateLifestealModifier(SourcedDamage<Actor> damage)
        {
            var args = new AttackLifestealEventArgs(damage);
            OnLifestealModifiers(args);
            return args.LifestealAmount;
        }

        protected void OnLifestealModifiers(AttackLifestealEventArgs e) => LifestealModifiers.Invoke(this, e);


        protected SourcedDamage<Actor> CalculateCritDamage(SourcedDamage<Actor> damage)
        {
            var args = new AttackCritEventArgs(damage);
            OnCritModifiers(args);
            return args.FinalDamage;
        }

        protected void OnCritModifiers(AttackCritEventArgs e) => CritModifiers.Invoke(this, e);


        protected void OnAttacking(AttackerableEventArgs e)        =>            Attacking?.Invoke(this, e);
        

        protected void OnAttacked(AttackerableEventArgs e)        =>            Attacked?.Invoke(this, e);
        

        public void Initialize(IAttackerableData data)
        {
            _damage.Value.Base = data.AttackDamage;
            _range.Value.Base = data.AttackRange;
            _attacksPerInterval.Value.Base = data.AttackSpeed;
            IsRanged = data.IsRanged;
            _interval.Value.Base = data.AttackInterval;
        }

        private void OnPreStep(float deltaTime)
        {
            if (!_attackCooldown.Done)
                _attackCooldown.AdvanceTime(deltaTime);
        }

        private void OnPhysicsStep(float deltaTime)
        {
            _trigger.Trigger.Collider.radius = _range.Value.Total;
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

		public void Register(IModifiable source)
		{
            _damage.Register(source);
            _interval.Register(source);
            _range.Register(source);
            _attacksPerInterval.Register(source);
		}

		public void Unregister(IModifiable source)
        {
            _damage.Unregister(source);
            _interval.Unregister(source);
            _range.Unregister(source);
            _attacksPerInterval.Unregister(source);
        }
	}
}