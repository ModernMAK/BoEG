using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    /// <summary>
    /// Helper type for Modules
    /// Allows any inheritors to access the actor it belongs to, as well as the Killable type.
    /// Useful for components which might need to check if dead before being performed.
    /// </summary>
    public class KillableActorModule : ActorModule
    {
        private readonly IKillable _killable;
        /// <summary>
        /// Creates a helper module for the actor.
        /// Allows modules to access their Actor, and the IKillable module.
        /// </summary>
        /// <param name="actor">The actor which owns this module.</param>
        /// <param name="killable">A killable module belonging to the actor.</param>
        public KillableActorModule(Actor actor, IKillable killable) : base(actor)
        {
            _killable = killable;
        }
        
        /// <summary>
        /// Creates a helper module for the actor.
        /// Allows modules to access their Actor, and the IKillable module.
        /// Will try and fetch a killable module from the actor, assigns null if not found.
        /// </summary>
        /// <param name="actor">The actor which owns this module.</param>
        public KillableActorModule(Actor actor) : this(actor, actor.GetModule<IKillable>())
        {
        }

        protected IKillable Killable => _killable;
        /// <summary>
        /// Returns the underlying Killable's IsDead property.
        /// </summary>
        /// <inheritdoc cref="IKillable.IsDead"/>
        /// <exception cref="NullReferenceException">Killable is null.</exception>
        protected bool IsDead => Killable.IsDead;
    }

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