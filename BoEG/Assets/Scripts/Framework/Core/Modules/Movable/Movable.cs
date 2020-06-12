using UnityEngine;
using UnityEngine.AI;

namespace Framework.Core.Modules
{
    [DisallowMultipleComponent]
    public class Movable : MonoBehaviour, IComponent<IMovableData>, IMovable
    {
        private IMovable _movable;

        public void Initialize(IMovableData module)
        {
            MoveSpeed = module.MoveSpeed;
            TurnSpeed = module.TurnSpeed;
            _agent = GetComponent<NavMeshAgent>();
        }

        private NavMeshAgent _agent;
        

        public float MoveSpeed { get; private set; }
        public float TurnSpeed { get; private set; }

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

        public bool HasReachedDestination => (_agent.destination - _agent.nextPosition).sqrMagnitude <= 0.01f;

        public void UpdateMover()
        {
            _agent.speed = MoveSpeed;
            _agent.acceleration = MoveSpeed * 1000;
            _agent.angularSpeed = TurnSpeed;
        }

        private void LateUpdate()
        {
            UpdateMover();
        }
    }
}