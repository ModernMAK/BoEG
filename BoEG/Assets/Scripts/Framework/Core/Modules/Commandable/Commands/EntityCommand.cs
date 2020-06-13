using UnityEngine;

namespace Framework.Core.Modules.Commands
{
    public abstract class EntityCommand : BaseCommand
    {
        public EntityCommand(GameObject entity)
        {
            Entity = entity;
        }

        protected GameObject Entity { get; }

        protected T GetComponent<T>()
        {
            return Entity.GetComponent<T>();
        }

        protected void GetComponent<T>(out T component)
        {
            component = GetComponent<T>();
        }
    }
}