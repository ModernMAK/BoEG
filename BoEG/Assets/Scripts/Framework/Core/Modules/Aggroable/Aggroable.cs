using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class Aggroable : IAggroable
    {
//        private static Collider[] _PhysicsBuffer = new Collider[256];
//
//        protected void Instantiate()
//        {
////            GetData(out _data);
//            _aggroTargets = new List<GameObject>();
//        }
//
//        private IAggroableData _data;
//        private List<GameObject> _aggroTargets;
//
//        public float AggroRange
//        {
//            get { return IsInitialized ? _data.AggroRange : 0f; }
//        }
//
//
//        public bool InAggro(GameObject go)
//        {
//            var delta = go.transform.position - transform.position;
//            return delta.sqrMagnitude <= AggroRange * AggroRange;
//        }
//
//        public IEnumerable<GameObject> GetAggroTargets()
//        {
//            return _aggroTargets;
//        }
//        public bool HasAggroTarget()
//        {
//            return _aggroTargets.Count >0;
//        }
//
//        /// <summary>
//        /// NOT THREAD SAFE
//        /// </summary>
//        /// <param name="deltaStep"></param>
//        protected override void PhysicsStep(float deltaStep)
//        {
//            var size = Physics.OverlapSphereNonAlloc(transform.position, AggroRange, _PhysicsBuffer);
//            _aggroTargets.Clear();
//            for (var i = 0; i < size; i++)
//                _aggroTargets.Add(_PhysicsBuffer[i].gameObject);
//            _aggroTargets.Sort((x, y) =>
//            {
//                var pos = transform.position;
//                var xDelta = (x.transform.position - pos);
//                var yDelta = (y.transform.position - pos);
//                return xDelta.sqrMagnitude.CompareTo(yDelta.sqrMagnitude);
//            });
//        }
        public float AggroRange => throw new NotImplementedException();

        public bool InAggro(GameObject go)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GameObject> GetAggroTargets()
        {
            throw new NotImplementedException();
        }

        public bool HasAggroTarget()
        {
            throw new NotImplementedException();
        }
    }
}