using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{

	/// <summary>
	/// Helper type for Modules
	/// Allows any inheritors to access the actor it belongs to.
	/// </summary>
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