using MobaGame.Framework.Core;
using UnityEngine;

namespace MobaGame.Entity.AI
{
    public class ActorBehaviour : MonoBehaviour
    {
        protected Actor Self { get; private set; }

        protected virtual void Awake()
        {
            Self = GetComponent<Actor>();
            if(Self == null)
                throw new MissingComponentException("An Actor Behaviour is not attached to an actor!");
        }
    }
}