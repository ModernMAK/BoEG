//using System;
//using UnityEngine;
//
//namespace Triggers
//{
////    public class Trigger : MonoBehaviour
////    {
////        private void Awake()
////        {
////        }
////
////        private void OnTriggerEnter(Collider col)
////        {
////            if (Enter != null)
////                Enter(GetGameObject(col));
////        }
////
////        private void OnTriggerExit(Collider col)
////        {
////            if (Exit != null)
////                Exit(GetGameObject(col));
////        }
////
////        private GameObject GetGameObject(Collider col)
////        {
////            return (col.attachedRigidbody != null) ? col.attachedRigidbody.gameObject : col.gameObject;
////        }
////
////        public event TriggerHandler Enter;
////        public event TriggerHandler Exit;
////    }
//
//    public class TriggerInstanceUtil
//    {
//        public static CollideHandler SphereCollision(GameObject parent, float radius,
//            int layerMask = Physics.DefaultRaycastLayers)
//        {
//            return SphereCollision(parent.transform, radius, layerMask);
//        }
//
//        public static CollideHandler SphereCollision(Transform parent, float radius,
//            int layerMask = Physics.DefaultRaycastLayers)
//        {
//            return SphereCollision(parent.position, radius, layerMask);
//        }
//
//        public static CollideHandler SphereCollision(Vector3 position, float radius,
//            int layerMask = Physics.DefaultRaycastLayers)
//        {
//            return () => Physics.OverlapSphere(position, radius, layerMask);
//        }
//    }
//}