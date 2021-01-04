using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Trigger
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

        public static bool TryGetTrigger<T>(Transform parent, string name, out TriggerHelper<T> result)
            where T : Collider
        {
            result = default;

            var transform = parent.Find(name);
            if (transform == null)
                return false;

            if (!transform.TryGetComponent<T>(out var collider))
                return false;

            if (!transform.TryGetComponent<Trigger>(out var trigger))
                return false;


            result = new TriggerHelper<T>()
            {
                Collider = collider,
                Trigger = trigger
            };
            return true;
        }

        public static TriggerHelper<T> CreateTrigger<T>(Transform parent, string name) where T : Collider
        {
            var triggerObj = CreateChild(parent, name).gameObject;
            triggerObj.layer = (int) Layer.Trigger;
            var collider = triggerObj.AddComponent<T>();
            var trigger = triggerObj.AddComponent<Trigger>();
            return new TriggerHelper<T>()
            {
                Collider = collider,
                Trigger = trigger
            };
        }

        public static TriggerHelper<T> CreateTrigger<T>(Actor actor, string name) where T : Collider
        {
            var container = GetTriggerContainer(actor);
            return CreateTrigger<T>(container, name);
        }

        public static TriggerHelper<T> GetOrCreateTrigger<T>(Actor actor, string name) where T : Collider
        {
            var container = GetTriggerContainer(actor);
            return GetOrCreateTrigger<T>(container, name);
        }

        public static TriggerHelper<T> GetOrCreateTrigger<T>(Transform parent, string name) where T : Collider
        {
            if (!TryGetTrigger<T>(parent, name, out var result))
                result = CreateTrigger<T>(parent, name);
            return result;
        }
    }
}