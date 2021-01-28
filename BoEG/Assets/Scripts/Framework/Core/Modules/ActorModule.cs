using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public class KillableActorModule : ActorModule
	{
        private readonly IKillable _killable;

		public KillableActorModule(Actor actor, IKillable killable) : base(actor)
		{
			_killable = killable;
		}

		protected IKillable Killable => _killable;

	}
    public class ActorModule
    {
        private readonly Actor _actor;
        protected Actor Actor => _actor;
        protected GameObject GameObject => _actor.gameObject;
        protected Transform Transform => _actor.transform;

        public ActorModule(Actor actor)
        {
            _actor = actor;
        }
    }
}