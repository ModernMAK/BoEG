using UnityEngine;

namespace Framework.Core.Modules.Commands
{
    public class EntityCommand : Command
    {
        protected GameObject Entity { get; private set; }

        public EntityCommand(GameObject entity)
        {
            Entity = entity;
        }

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