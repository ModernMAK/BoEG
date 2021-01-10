using System.Collections.Generic;
using Framework.Core;
using MobaGame.Framework.Core.Trigger;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public class AggroableModule : MonoBehaviour, IAggroable, IInitializable<IAggroableData>, IListener<IStepableEvent>
    {
        [SerializeField] private Aggroable _aggroable;

        private void Awake()
        {
            var actor = GetComponent<Actor>();
            var teamable = this.GetModule<ITeamable>();
            _aggroable = new Aggroable(actor, teamable);
        }

        public float AggroRange => _aggroable.AggroRange;


        public IReadOnlyList<Actor> GetAggroTargets() => _aggroable.GetAggroTargets();

        public Actor GetAggroTarget(int index) => _aggroable.GetAggroTarget(index);

        public bool HasAggroTarget() => _aggroable.HasAggroTarget();

        public void Initialize(IAggroableData module) => _aggroable.Initialize(module);


        public void Register(IStepableEvent source) => _aggroable.Register(source);

        public void Unregister(IStepableEvent source) => _aggroable.Unregister(source);
    }
}