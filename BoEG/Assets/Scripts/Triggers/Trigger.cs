using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Triggers
{
    public class Trigger
    {
        public Trigger(TriggerMethod handler)
        {
            _colliders = new List<GameObject>();
            CollisionMethod = handler;
        }

        private static Collider[] DefaultCollisionMethod()
        {
            return new Collider[0];
        }

        private TriggerMethod CollisionMethod { get; set; }

        private GameObject[] TriggerStep()
        {
            var ret = CollisionMethod.Collide();
            return GetGameObjectFromColliders(ret);
        }
		public static GameObject[] GetGameObjectFromColliders(Collider[] cols)
		{
			var gos = new GameObject[cols.Length];
            var counter = 0;
            foreach (var col in cols)
            {
                gos[counter] = (col.attachedRigidbody != null) ? col.attachedRigidbody.gameObject : col.gameObject;
                counter++;
            }
            return gos;
		}

        public void PhysicsStep()
        {
            //Get all collisions
            var collisions = TriggerStep();
            //Get all who entered this step
            var entered = collisions.Except(_colliders).ToArray();
            //Get all who stayed on this step
            var stayed = collisions.Intersect(_colliders).ToArray();
            //Get all who exited this step
            var exited = _colliders.Except(collisions).ToArray();

            foreach (var enter in entered)
            {
                OnEnter(enter);
            }

            foreach (var stay in stayed)
            {
                OnStay(stay);
            }

            foreach (var exit in exited)
            {
                OnExit(exit);
            }
        }


        private readonly List<GameObject> _colliders;
        public IEnumerable<GameObject> Colliders
        {
            get { return _colliders; }
        }

        private void OnEnter(GameObject go)
        {
            _colliders.Add(go);
            if (Enter != null)
                Enter(go);
        }

        private void OnStay(GameObject go)
        {
            if (Stay != null)
                Stay(go);
        }

        private void OnExit(GameObject go)
        {
            _colliders.Remove(go);
            if (Exit != null)
                Exit(go);
        }

        public event TriggerHandler Enter;
        public event TriggerHandler Stay;
        public event TriggerHandler Exit;
    }
}