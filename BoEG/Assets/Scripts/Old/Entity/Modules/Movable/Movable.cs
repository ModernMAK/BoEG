//using UnityEngine;
//using UnityEngine.AI;
//

using System;
using Old.Util;
using UnityEngine;
using UnityEngine.AI;

namespace Old.Entity.Modules.Movable
{
    [Serializable]
    [DisallowMultipleComponent]
    public class Movable : BuffedModule, IMovable
    {
        [SerializeField] private IMovableData _data;
        private NavMeshAgent _agent;

        protected override void Initialize()
        {
            _data = GetData<IMovableData>();
            _agent = GetComponent<NavMeshAgent>();
        }

        public float MoveSpeed
        {
            get { return _data.BaseMoveSpeed; }
        }

        public float TurnSpeed
        {
            get { return _data.BaseTurnSpeed; }
        }

        public bool MoveTo(Vector3 destenation)
        {
//            Debug.Log(destenation);
            return _agent.SetDestination(destenation);
        }

        public void Push(Vector3 direction)
        {
            _agent.Move(direction);
        }

        public bool Teleport(Vector3 destenation)
        {
            return _agent.Warp(destenation);
        }

        public void Pause()
        {
            _agent.isStopped = true;
        }

        public void Resume()
        {
            _agent.isStopped = false;
        }

        public void Stop()
        {
            _agent.ResetPath();
        }

        protected override void PreTick()
        {
            base.PreTick();
            _agent.acceleration = 100;
            _agent.speed = MoveSpeed;
            _agent.angularSpeed = 360 * TurnSpeed;
        }
        
        public delegate void DELEGATE();

        public event DELEGATE MovedEvent;

        protected override void PostTick()
        {
            if (_agent.pathPending || !(_agent.remainingDistance <= _agent.stoppingDistance)) return;
            if (!_agent.hasPath || _agent.velocity.sqrMagnitude.SafeEquals(0f) &&
                _agent.steeringTarget.sqrMagnitude.SafeEquals(0f))
            {
//                Stop();
            }
        }
    }
    
}