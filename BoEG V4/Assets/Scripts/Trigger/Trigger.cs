using UnityEngine;

namespace Trigger
{
    [RequireComponent(typeof(Collider))]
    public class Trigger : UnetBehaviour
    {
        private TriggerStrategy _stratagy;

        public TriggerStrategy Strategy
        {
            get { return _stratagy; }
            set { _stratagy = value ?? new TriggerStrategy(); }
        }

        protected override void Awake()
        {
            base.Awake();
            Strategy = null;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (Strategy.ShouldEnter(other.gameObject))
                Strategy.Enter(other.gameObject);
        }

        public void OnTriggerStay(Collider other)
        {
            if (Strategy.ShouldStay(other.gameObject))
                Strategy.Stay(other.gameObject);
        }

        public void OnTriggerExit(Collider other)
        {
            if (Strategy.ShouldExit(other.gameObject))
                Strategy.Exit(other.gameObject);
        }
    }

    public static class TriggerExt
    {
        public static T CreateTrigger<T>(this Component parent, TriggerStrategy strategy, string name = "Trigger")
            where T : Trigger
        {
            return parent.transform.CreateTrigger<T>(strategy, name);
        }

        public static T CreateTrigger<T>(this Transform parent, TriggerStrategy strategy, string name = "Trigger")
            where T : Trigger
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent.transform);
            go.transform.localPosition = Vector3.zero;
            var trigger = go.AddComponent<T>();
            trigger.Strategy = strategy;
            return trigger;
        }
    }
}