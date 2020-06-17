using System;
using System.Collections.Generic;
using Entity.Abilities.FlameWitch;
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
            _triggerHelper = TriggerUtility.CreateTrigger<SphereCollider>(transform, "Aggroable Trigger");
            _teamable = GetComponent<ITeamable>();
            _aggroTarget = new List<GameObject>();
            _triggerHelper.Trigger.Enter += TriggerOnEnter;
            _triggerHelper.Trigger.Exit += TriggerOnExit;
        }

        private void TriggerOnExit(object sender, TriggerEventArgs e)
        {
            var go = e.Collider.gameObject;
            _aggroTarget.Remove(go);
        }

        private void TriggerOnEnter(object sender, TriggerEventArgs e)
        {
            var go = e.Collider.gameObject;
            if (go == gameObject)
                return;
            if (_aggroTarget.Contains(go))
                return;
            if (!go.TryGetComponent<Actor>(out var actor))
                return;
            if (_teamable != null && go.TryGetComponent<ITeamable>(out var teamable))
            {
                if (_teamable.SameTeam(teamable))
                    return;
            }

            _aggroTarget.Add(go);
        }

        private float _aggroRange;
        public float AggroRange => _aggroRange;
        private List<GameObject> _aggroTarget;
        private ITeamable _teamable;

        public bool InAggro(GameObject go)
        {
            return AbilityHelper.InRange(transform, go.transform.position, AggroRange);
        }

        public IReadOnlyList<GameObject> GetAggroTargets() => _aggroTarget;

        public GameObject GetAggroTarget(int index) => _aggroTarget[index];

        public bool HasAggroTarget() => _aggroTarget.Count > 0;

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
            for (var i = 0; i < _aggroTarget.Count; i++)
            {
                var target = _aggroTarget[i];
                if (!target.activeSelf)
                {
                    _aggroTarget.RemoveAt(i);
                    i--;
                    continue;
                }

                if (target.TryGetComponent<ITeamable>(out var teamable))
                {
                    if (_teamable.SameTeam(teamable))
                    {
                        _aggroTarget.RemoveAt(i);
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