using System;
using System.Collections.Generic;
using Entity.Abilities.FlameWitch;
using Framework.Ability;
using Framework.Types;
using Modules.Teamable;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class Aggroable : MonoBehaviour, IAggroable, IInitializable<IAggroableData>, IListener<IStepableEvent>
    {
        private TriggerHelper<SphereCollider> _triggerHelper;

        private void Awake()
        {
            var actor = GetComponent<Actor>();
            _triggerHelper = TriggerUtility.CreateTrigger<SphereCollider>(actor, "Aggroable Trigger");
            _teamable = GetComponent<ITeamable>();
            Targets = new List<Actor>();
            _triggerHelper.Trigger.Enter += TriggerOnEnter;
            _triggerHelper.Trigger.Exit += TriggerOnExit;
        }

        private void TriggerOnExit(object sender, TriggerEventArgs e)
        {
            var go = e.Collider.gameObject;

            if (!go.TryGetComponent<Actor>(out var actor))
                return;
            Targets.Remove(actor);
        }

        private void TriggerOnEnter(object sender, TriggerEventArgs e)
        {
            var go = e.Collider.gameObject;
            if (go == gameObject)
                return;
            if (!go.TryGetComponent<Actor>(out var actor))
                return;

            if (Targets.Contains(actor))
                return;
            if (_teamable != null && go.TryGetComponent<ITeamable>(out var teamable))
            {
                if (_teamable.SameTeam(teamable))
                    return;
            }

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


        private float _aggroRange;
        public float AggroRange => _aggroRange;
        private List<Actor> Targets { get; set; }

        private ITeamable _teamable;


        public IReadOnlyList<Actor> GetAggroTargets() => Targets;

        public Actor GetAggroTarget(int index) => Targets[index];

        public bool HasAggroTarget() => Targets.Count > 0;

        public void Initialize(IAggroableData module)
        {
            _aggroRange = module.AggroRange;
        }

        private void OnPreStep(float deltaStep)
        {
            _triggerHelper.Collider.radius = AggroRange;
        }

        private void OnPhysicsStep(float deltaStep)
        {
            if (_teamable == null)
                return;
            for (var i = 0; i < Targets.Count; i++)
            {
                var target = Targets[i];
                if (target == null)
                {
                    Targets.RemoveAt(i);
                    i--;
                    continue;
                }

                if (!target.gameObject.activeInHierarchy)
                {
                    Targets.RemoveAt(i);
                    i--;
                    continue;
                }

                if (target.TryGetComponent<ITeamable>(out var teamable))
                {
                    if (_teamable.SameTeam(teamable))
                    {
                        Targets.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
            }
        }

        public void Register(IStepableEvent source)
        {
            source.PhysicsStep += OnPhysicsStep;
            source.PreStep += OnPreStep;
        }

        public void Unregister(IStepableEvent source)
        {
            source.PreStep -= OnPreStep;
            source.PhysicsStep -= OnPhysicsStep;
        }
    }
}