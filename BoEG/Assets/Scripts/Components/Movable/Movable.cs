//using UnityEngine;
//using UnityEngine.AI;
//

using System;
using Core;
using UnityEngine;
using UnityEngine.AI;

namespace Components.Movable
{
    [Serializable]
    public class Movable : BuffedModule, IMovable
    {
        public Movable(IMovableData data)
        {
            _data = data;
        }

        [SerializeField] private IMovableData _data;
        private NavMeshAgent _agent;

        public override void Initialize(Entity e)
        {
            base.Initialize(e);
            _agent = e.GetComponent<NavMeshAgent>();
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

        public override void PreTick()
        {
            base.PreTick();
            _agent.acceleration = 100;
            _agent.speed = MoveSpeed;
            _agent.angularSpeed = 360 * TurnSpeed;
        }

        public override void PostTick()
        {
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude.SafeEquals(0f) &&
                    _agent.steeringTarget.sqrMagnitude.SafeEquals(0f))
                {
                    Stop();
                }
        }
    }
}