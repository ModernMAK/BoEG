using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    public static class ColliderExtensions
    {
        public static Collider[] OverlapSphere(this SphereCollider col) =>
            Physics.OverlapSphere(col.transform.position + col.center, col.radius);

        public static Collider[] OverlapSphere(this SphereCollider col, int layerMask) =>
            Physics.OverlapSphere(col.transform.position + col.center, col.radius, layerMask);

        public static Collider[] OverlapSphere(this SphereCollider col, int layerMask, QueryTriggerInteraction query) =>
            Physics.OverlapSphere(col.transform.position + col.center, col.radius, layerMask, query);


        public static Collider[] OverlapBox(this BoxCollider col) =>
            Physics.OverlapBox(col.transform.position + col.center, col.size / 2f, col.transform.rotation);

        public static Collider[] OverlapBox(this BoxCollider col, int layerMask) =>
            Physics.OverlapBox(col.transform.position + col.center, col.size / 2f, col.transform.rotation, layerMask);

        public static Collider[] OverlapBox(this BoxCollider col, int layerMask, QueryTriggerInteraction query) =>
            Physics.OverlapBox(col.transform.position + col.center, col.size / 2f, col.transform.rotation, layerMask,
                query);


        private static void GetCapsulePoints(this CapsuleCollider col, out Vector3 p0, out Vector3 p1, out float radius)
        {
            Vector3 axis;
            switch (col.direction)
            {
                //X
                case 0:
                    axis = Vector3.right;
                    break;
                case 1:
                    axis = Vector3.up;
                    break;
                case 2:
                    axis = Vector3.forward;
                    break;
                default:
                    throw new ArgumentException();
            }

            p0 = col.center - col.height * axis / 2;
            p1 = col.center + col.height * axis / 2;
            radius = col.radius;

            var rot = col.transform.rotation;
            p0 = rot * p0;
            p1 = rot * p1;
            var pos = col.transform.position;
            p0 += pos;
            p1 += pos;
        }

        public static Collider[] OverlapCapsule(this CapsuleCollider col)
        {
            GetCapsulePoints(col, out var p0, out var p1, out var radius);
            return Physics.OverlapCapsule(p0, p1, radius);
        }

        public static Collider[] OverlapCapsule(this CapsuleCollider col, int layerMask)
        {
            GetCapsulePoints(col, out var p0, out var p1, out var radius);
            return Physics.OverlapCapsule(p0, p1, radius, layerMask);
        }


        public static Collider[] OverlapCapsule(this CapsuleCollider col, int layerMask, QueryTriggerInteraction query)
        {
            GetCapsulePoints(col, out var p0, out var p1, out var radius);
            return Physics.OverlapCapsule(p0, p1, radius, layerMask, query);
        }
    }
}