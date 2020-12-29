using System;
using System.Collections.Generic;
using Framework.Types;
using Modules.Teamable;
using UnityEngine;

namespace Framework.Core.Modules
{
    [DisallowMultipleComponent]
    public class Attackerable : MonoBehaviour, IAttackerable, IInitializable<IAttackerableData>,
        IListener<IStepableEvent>
    {
        private void Awake()
        {
            var actor = GetComponent<Actor>();
            _teamable = GetComponent<ITeamable>();
            _triggerHelper = TriggerUtility.CreateTrigger<SphereCollider>(actor, "Attackerable Trigger");
            _targets = new List<Actor>();
            _triggerHelper.Trigger.Enter += TriggerOnEnter;
            _triggerHelper.Trigger.Exit += TriggerOnExit;
        }


        private void TriggerOnExit(object sender, TriggerEventArgs e)
        {
            var go = e.Collider.gameObject;

            if (!go.TryGetComponent<Actor>(out var actor))
                return;
            _targets.Remove(actor);
        }

        private void TriggerOnEnter(object sender, TriggerEventArgs e)
        {
            var go = e.Collider.gameObject;
            if (!go.TryGetComponent<Actor>(out var actor))
                return;
            if (Targets.Contains(actor))
                return;
            if (_teamable != null && go.TryGetComponent<ITeamable>(out var teamable))
                if (_teamable.SameTeam(teamable))
                    return;


            if (go.TryGetComponent<IHealthable>(out var healthble))
                healthble.Died += TargetDied;
            Targets.Add(actor);
        }


        private void TargetDied(object sender, DeathEventArgs e)
        {
            var healthable = sender as IHealthable;
            healthable.Died -= TargetDied;
            Targets.Remove(e.Self);
        }

        private List<Actor> _targets;
        private IList<Actor> Targets => _targets;


        private float _cooldownEnd = float.MinValue;
        private TriggerHelper<SphereCollider> _triggerHelper;
        private ITeamable _teamable;


        public float AttackDamage { get; protected set; }
        public float AttackRange { get; protected set; }
        public float AttackSpeed { get; protected set; }

        public float AttackCooldown => AttackInterval / AttackSpeed;

        public float AttackInterval { get; protected set; }

        public bool IsRanged { get; protected set; }

        public bool IsAttackOnCooldown => (Time.time <= _cooldownEnd);


        private void PutAttackOnCooldown() => _cooldownEnd = Time.time + AttackCooldown;

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
        }

        //TODO drop this; use DeadEvent and a TeamChanged Event
        private void OnPhysicsStep(float deltaTime)
        {
            for (var i = 0; i < _targets.Count; i++)
            {
                var actor = _targets[i];
                if (actor == null)
                {
                    _targets.RemoveAt(i);
                    i--;
                    continue;
                }

                if (!actor.gameObject.activeSelf)
                {
                    _targets.RemoveAt(i);
                    i--;
                    continue;
                }

                if (_teamable != null && actor.TryGetComponent<ITeamable>(out var teamable))
                    if (_teamable.SameTeam(teamable))
                    {
                        _targets.RemoveAt(i);
                        i--;
                        continue;
                    }
            }
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
    }
}