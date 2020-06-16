using Triggers;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class TriggerHelper<T> where T : Collider
    {
        public T Collider { get; set; }
        public Trigger Trigger { get; set; }
    }

    public static class TriggerUtility
    {
        public static Transform GetTriggerContainer(Actor actor)
        {
            const string containerName = "Trigger Container";
            foreach (Transform child in actor.transform)
            {
                if (child.name == containerName)
                {
                    return child;
                }
            }

            return CreateChild(actor.transform, containerName);
        }

        public static Transform CreateChild(Transform parent, string name)
        {
            var child = new GameObject(name);
            child.transform.SetParent(parent);
            child.transform.localPosition = Vector3.zero;
            return child.transform;
        }

        public static TriggerHelper<T> CreateTrigger<T>(Transform parent, string name) where T : Collider
        {
            var triggerObj = CreateChild(parent, name).gameObject;
            triggerObj.layer = (int)Layer.Trigger;
            var collider = triggerObj.AddComponent<T>();
            var trigger = triggerObj.AddComponent<Trigger>();
            return new TriggerHelper<T>()
            {
                Collider = collider,
                Trigger = trigger
            };
        }
    }
}