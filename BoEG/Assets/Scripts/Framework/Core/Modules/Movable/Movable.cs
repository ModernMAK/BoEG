using UnityEngine;
using UnityEngine.AI;

namespace Framework.Core.Modules
{
    [DisallowMultipleComponent]
    public class Movable : MonoBehaviour, IInitializable<IMovableData>, IMovable
    {
        private NavMeshAgent _agent;
        private IMovable _movable;

        public void Initialize(IMovableData module)
        {
            MoveSpeed = module.MoveSpeed;
            TurnSpeed = module.TurnSpeed;
            _agent = GetComponent<NavMeshAgent>();
        }


        public float MoveSpeed { get; private set; }
        public float TurnSpeed { get; private set; }

        public void MoveTo(Vector3 destenation)
        {
            if (_agent.gameObject.activeSelf)
                _agent.SetDestination(destenation);
        }

        public void WarpTo(Vector3 destenation)
        {
            if (_agent.gameObject.activeSelf)
                _agent.Warp(destenation);
        }

        public void Push(Vector3 direction)
        {
            if (_agent.gameObject.activeSelf)
                _agent.Move(direction);
        }

        public void StopMovement()
        {
            if (_agent.gameObject.activeSelf)
                _agent.isStopped = true;
        }

        public void StartMovement()
        {
            if (_agent.gameObject.activeSelf)
                _agent.isStopped = false;
        }

        public void CancelMovement()
        {
            if (_agent.gameObject.activeSelf)
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