using System;
using Core;
using Old.Util;
using UnityEngine;
using UnityEngine.AI;

namespace Modules.Movable
{
    [Serializable]
//    [DisallowMultipleComponent]
    public class Movable : Module, IMovable
    {
        private readonly IMovableData _data;
        private readonly NavMeshAgent _agent;

        public Movable(GameObject self, IMovableData data) : base(self)
        {
            _data = data;
            _agent = self.GetComponent<NavMeshAgent>();
        }

        public override void Initialize()
        {
        }

        public override void PreTick(float deltaTime)
        {
            _agent.acceleration = 100;
            _agent.speed = MoveSpeed;
            _agent.angularSpeed = 360 * TurnSpeed;
        }

        public override void PostTick(float deltaTime)
        {
            if(!_agent.isActiveAndEnabled)
                return;
            
            if (_agent.pathPending || !(_agent.remainingDistance <= _agent.stoppingDistance)) return;
            if (!_agent.hasPath || _agent.velocity.sqrMagnitude.SafeEquals(0f) &&
                _agent.steeringTarget.sqrMagnitude.SafeEquals(0f))
            {
//                Stop();
            }
        }

        public float MoveSpeed
        {
            get { return _data.MoveSpeed; }
        }

        public float TurnSpeed
        {
            get { return _data.TurnSpeed; }
        }

        public bool MoveTo(Vector3 destenation)
        {
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

//        protected override void PreTick()
//        {
//            base.PreTick();
//            _agent.acceleration = 100;
//            _agent.speed = MoveSpeed;
//            _agent.angularSpeed = 360 * TurnSpeed;
//        }


        public event DEFAULT_HANDLER MovedEvent;
//
//        protected override void PostTick()
//        {
//        }
    }
}