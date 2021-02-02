using Framework.Core;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Commands
{
    public abstract class ActorCommand : BaseCommand
	{

        public ActorCommand(Actor entity)
        {
            Actor = entity;
        }

        protected Actor Actor { get; }

        protected T GetModule<T>()        =>             Actor.GetModule<T>();
        

        protected void GetModule<T>(out T component)=>           component = GetModule<T>();
        
    }
    public abstract class GameObjectCommand : ActorCommand
    {
        public GameObjectCommand(GameObject entity) : base(entity.GetComponent<Actor>())
        {
            Entity = entity;
        }

        protected GameObject Entity { get; }

    }
}