using Framework.Core;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Commands
{
    public abstract class EntityCommand : BaseCommand
    {
        public EntityCommand(GameObject entity)
        {
            Entity = entity;
        }

        protected GameObject Entity { get; }

        protected T GetModule<T>()
        {
            return Entity.GetModule<T>();
        }

        protected void GetModule<T>(out T component)
        {
            component = GetModule<T>();
        }
    }
}