using System.Collections.Generic;
using MobaGame.Framework.Core.Trigger;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public class Aggroable : ActorModule, IAggroable, IInitializable<IAggroableData>, IListener<IStepableEvent>
    {
        #region Constants / Statics

        public const string TriggerName = "Aggroable Trigger";
        
        #endregion

        #region Constructors

        public Aggroable(Actor actor, ITeamable teamable = default) : base(actor)
        {
            var helper = TriggerUtility.CreateTrigger<SphereCollider>(actor, TriggerName);
            _triggerLogic = new AttackTargetTrigger<SphereCollider>(Actor, helper, teamable);
        }

        

        #endregion
        
        # region Variables
        private float _searchRange;
        private readonly AttackTargetTrigger<SphereCollider> _triggerLogic;
        #endregion

        #region Properties

        public float SearchRange => _searchRange;
        public IReadOnlyList<Actor> Targets => _triggerLogic.Targets;
        

        #endregion


        #region IInitializable

        public void Initialize(IAggroableData data)
        {
            _searchRange = data.AggroRange;
            _triggerLogic.Trigger.Collider.radius = _searchRange;
        }        

        #endregion


        #region IListener<ISteppableEvent>

        

        private void OnPhysicsStep(float deltaStep)
        {
            _triggerLogic.Trigger.Collider.radius = SearchRange;
        }

        public void Register(IStepableEvent source)
        {
            source.PhysicsStep += OnPhysicsStep;
        }

        public void Unregister(IStepableEvent source)
        {
            source.PhysicsStep -= OnPhysicsStep;
        }
        #endregion
    }
}