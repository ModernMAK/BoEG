using System.Collections.Generic;
using MobaGame.Framework.Core.Trigger;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public class Aggroable : ActorModule, IAggroable, IInitializable<IAggroableData>, IListener<IStepableEvent>
    {
        public const string AggroableTriggerName = "Aggroable Trigger";


        private float _aggroRange;

        public float AggroRange => _aggroRange;
        private readonly AttackTargetTrigger<SphereCollider> _triggerLogic;
        private IReadOnlyList<Actor> Targets => _triggerLogic.Targets;

        public Aggroable(Actor actor, ITeamable teamable = default) : base(actor)
        {
            var helper = TriggerUtility.CreateTrigger<SphereCollider>(actor, AggroableTriggerName);
            _triggerLogic = new AttackTargetTrigger<SphereCollider>(Actor, helper, teamable);
        }


        public IReadOnlyList<Actor> GetAggroTargets() => Targets;

        public Actor GetAggroTarget(int index) => Targets[index];

        public bool HasAggroTarget() => Targets.Count > 0;

        public void Initialize(IAggroableData module)
        {
            _aggroRange = module.AggroRange;
        }

        private void OnPhysicsStep(float deltaStep)
        {
            _triggerLogic.Trigger.Collider.radius = AggroRange;
        }

        public void Register(IStepableEvent source)
        {
            source.PhysicsStep += OnPhysicsStep;
        }

        public void Unregister(IStepableEvent source)
        {
            source.PhysicsStep -= OnPhysicsStep;
        }
    }
}