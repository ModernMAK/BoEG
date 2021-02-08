using MobaGame.Framework.Core;
using UnityEngine;

namespace MobaGame.Entity.AI
{
    /// <summary>
    /// An abstract container type for a monobehaviour which references an Actor.
    /// </summary>
    public abstract class ActorBehaviour : MonoBehaviour
    {
        /// <summary>
        /// The attached actor.
        /// </summary>
        /// <remarks>
        /// Component is cached on awake; repeated calls will not hinder performance.
        /// </remarks>
        public Actor Self { get; private set; }

        /// <summary>
        /// Unity event, fired when the gameobject is created.
        /// </summary>
        /// <remarks>
        /// When overriden base.Awake() must be called to initialize Self and avoid NullReferenceExceptions.
        /// </remarks>
        /// <exception cref="MissingComponentException">The gameobject does not have a component inheriting from 'Actor' attached.</exception>
        protected virtual void Awake()
        {
            Self = GetComponent<Actor>();
            if(Self == null)
                throw new MissingComponentException("An Actor Behaviour is not attached to this gameobject!");
        }
    }
}