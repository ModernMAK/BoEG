using System;
using System.Collections.Generic;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Core.Trigger;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    [DisallowMultipleComponent]
    public class Attackerable : MonoBehaviour, IAttackerable, IInitializable<IAttackerableData>,
        IListener<IStepableEvent>, IRespawnable
    {
        private Actor _actor;

        public const string AttackerableTrigger = "Attackerable Trigger";

        private void Awake()
        {
            _actor = GetComponent<Actor>();
            _teamable = GetComponent<ITeamable>();
            _triggerHelper = TriggerUtility.CreateTrigger<SphereCollider>(_actor, AttackerableTrigger);
            _targets = new List<Actor>();
            _triggerHelper.Trigger.Enter += TriggerOnEnter;
            _triggerHelper.Trigger.Exit += TriggerOnExit;
            if (_teamable != null)
            {
                _teamable.TeamChanged += OnMyTeamChanged;
            }

            _attackCooldown = new DurationTimer(0f);
            _attackCooldown.SetDone();
        }

        private void OnMyTeamChanged(object sender, TeamData e)
        {
            InstantlyRebuildTargets();
        }

        private void InstantlyRebuildTargets()
        {
            while (_targets.Count > 0)
            {
                InternalRemoveActor(_targets[0]);
            }

            var colliders = _triggerHelper.Trigger.OverlapCollider((int) LayerMaskHelper.Entity);
            foreach (var col in colliders)
            {
                if (AbilityHelper.TryGetActor(col, out var actor))
                    InternalAddActor(actor);
            }
        }


        void InternalAddActor(Actor actor)
        {
            if (actor == _actor)
                return;
            if (Targets.Contains(actor))
                return;

            if (!actor.TryGetComponent<IDamageTarget>(out _))
                return;

            if (actor.TryGetComponent<IHealthable>(out var healthable))
                healthable.Died += TargetDied;

            if (actor.TryGetComponent<ITeamable>(out var teamable))
                teamable.TeamChanged += OnTargetTeamChanged;

            if (_teamable?.SameTeam(teamable) ?? false)
                return;

            _targets.Add(actor);
        }

        void InternalRemoveActor(Actor actor)
        {
            if (actor.TryGetComponent<IHealthable>(out var healthble))
                healthble.Died -= TargetDied;

            if (actor.TryGetComponent<ITeamable>(out var teamable))
                teamable.TeamChanged -= OnTargetTeamChanged;

            _targets.Remove(actor);
        }

        private void OnTargetTeamChanged(object sender, TeamData e)
        {
            InstantlyRebuildTargets();
        }


        private void TriggerOnExit(object sender, TriggerEventArgs e)
        {
            var go = e.Collider.gameObject;


            if (!go.TryGetComponent<Actor>(out var actor))
                return;

            InternalRemoveActor(actor);
        }

        private void TriggerOnEnter(object sender, TriggerEventArgs e)
        {
            var go = e.Collider.gameObject;
            if (!go.TryGetComponent<Actor>(out var actor))
                return;

            InternalAddActor(actor);
        }


        private void TargetDied(object sender, DeathEventArgs e)
        {
            InternalRemoveActor(e.Self);
        }

        private List<Actor> _targets;
        private IList<Actor> Targets => _targets;

        private DurationTimer _attackCooldown;
        private TriggerHelper<SphereCollider> _triggerHelper;
        private ITeamable _teamable;


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

        public IReadOnlyList<Actor> GetAttackTargets() => (IReadOnlyList<Actor>) Targets;

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
            _triggerHelper.Collider.radius = AttackRange;
            if (!_attackCooldown.Done)
                _attackCooldown.AdvanceTime(deltaTime);
        }


        public void Register(IStepableEvent source)
        {
            source.PreStep += OnPreStep;
            // source.PhysicsStep += OnPhysicsStep;
        }

        public void Unregister(IStepableEvent source)
        {
            source.PreStep -= OnPreStep;
            // source.PhysicsStep -= OnPhysicsStep;
        }

        public void Respawn()
        {
            _attackCooldown.SetDone();
        }
    }
}