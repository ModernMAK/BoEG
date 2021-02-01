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
    public abstract class GameObjectCommand : BaseCommand
    {
        public GameObjectCommand(GameObject entity)
        {
            Entity = entity;
            Actor = Entity.GetComponent<Actor>();
        }

        protected GameObject Entity { get; }
        protected Actor Actor { get; }

        protected T GetModule<T>()
        {
            return Actor.GetModule<T>();
        }

        protected void GetModule<T>(out T component)
        {
            component = GetModule<T>();
        }
    }
}