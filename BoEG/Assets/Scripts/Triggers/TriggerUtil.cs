//using UnityEngine;
//
//namespace Triggers
//{
//    public static class TriggerUtil
//    {
//        public static Trigger CreateTrigger(Vector3 postion, string name = "Trigger")
//        {
//            var go = new GameObject(name);
//            var trigger = go.AddComponent<Trigger>();
//            go.transform.position = postion;
//            return trigger;
//        }
//
//        public static Trigger CreateTrigger(GameObject parent, string name = "Trigger")
//        {
//            return CreateTrigger(parent.transform, name);
//        }
//
//        public static Trigger CreateTrigger(Transform parent, string name = "Trigger")
//        {
//            var go = new GameObject(name);
//            var trigger = go.AddComponent<Trigger>();
//            go.transform.parent = parent;
//            go.transform.localPosition = Vector3.zero;
//            return trigger;
//        }
//
//        private static void FixCollider(Collider col)
//        {
//            col.isTrigger = true;
//            col.gameObject.layer = (int) Layer.Trigger;
//        }
//
//        public static SphereCollider CreateRadialTrigger(Trigger trigger)
//        {
//            var collider = trigger.gameObject.AddComponent<SphereCollider>();
//            FixCollider(collider);
//            return collider;
//        }
//    }
//}