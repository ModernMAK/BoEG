using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Triggers
{
    public class Trigger
    {
        private readonly List<GameObject> _colliders;

        public Trigger(TriggerMethod handler)
        {
            _colliders = new List<GameObject>();
            CollisionMethod = handler;
        }

        private TriggerMethod CollisionMethod { get; }

        public IEnumerable<GameObject> Colliders => _colliders;

        private static Collider[] DefaultCollisionMethod()
        {
            return new Collider[0];
        }

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
                var attachedRigidbody = col.attachedRigidbody;
                gos[counter] = attachedRigidbody != null ? attachedRigidbody.gameObject : col.gameObject;
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

            foreach (var enter in entered) OnEnter(enter);

            foreach (var stay in stayed) OnStay(stay);

            foreach (var exit in exited) OnExit(exit);
        }

        private void AddCollider(GameObject go)
        {
            _colliders.Add(go);
//            if (Enter != null)
//                Enter(go);
            OnEnter(go);
        }

//        private void OnStay(GameObject go)
//        {
//            if (Stay != null)
//                Stay(go);
//        }

        private void RemoveCollider(GameObject go)
        {
            _colliders.Remove(go);
//            if (Exit != null)
//                Exit(go);
            OnExit(go);
        }

        public event EventHandler<GameObject> Enter;
        public event EventHandler<GameObject> Stay;
        public event EventHandler<GameObject> Exit;

        protected virtual void OnEnter(GameObject e)
        {
            Enter?.Invoke(this, e);
        }

        protected virtual void OnStay(GameObject e)
        {
            Stay?.Invoke(this, e);
        }

        protected virtual void OnExit(GameObject e)
        {
            Exit?.Invoke(this, e);
        }
    }
}