using System;
using Framework.Types;
using Framework.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace Framework.Core.Modules
{
    public class Movable : IMovable
    {
        public Movable(float moveSpeed, float turnSpeed, NavMeshAgent agent)
        {
            MoveSpeed = moveSpeed;
            TurnSpeed = turnSpeed;
            _agent = agent;
        }

        public Movable(IMovableData data, NavMeshAgent agent) : this(data.MoveSpeed, data.TurnSpeed, agent)
        {
        }

        private readonly NavMeshAgent _agent;


        private IMovableData _data;

        public float MoveSpeed { get; }
        public float TurnSpeed { get; }

        public void MoveTo(Vector3 destenation)
        {
            _agent.SetDestination(destenation);
        }

        public void WarpTo(Vector3 destenation)
        {
            _agent.Warp(destenation);
        }

        public void Push(Vector3 direction)
        {
            _agent.Move(direction);
        }

        public void StopMovement()
        {
            _agent.isStopped = true;
        }

        public void StartMovement()
        {
            _agent.isStopped = false;
        }

        public void CancelMovement()
        {
            _agent.ResetPath();
        }

        public bool HasReachedDestination
        {
            get { throw new NotImplementedException(); }
        }

        public void UpdateMover()
        {
            _agent.speed = MoveSpeed;
            _agent.acceleration = MoveSpeed * 1000;
            _agent.angularSpeed = TurnSpeed;
        }
    }
}