using UnityEngine;

namespace Framework.Core.Modules
{
    [AddComponentMenu("EndGame/Components/Movable")]
    [DisallowMultipleComponent]
    public class MovableComponent : MonoBehaviour, IComponent<IMovable>, IMovable
    {
        private IMovable _movable;

        public void Initialize(IMovable module)
        {
            _movable = module;
        }

        public float MoveSpeed => _movable.MoveSpeed;

        public float TurnSpeed => _movable.TurnSpeed;

        public void MoveTo(Vector3 destenation)
        {
            _movable.MoveTo(destenation);
        }

        public void WarpTo(Vector3 destenation)
        {
            _movable.WarpTo(destenation);
        }

        public void Push(Vector3 direction)
        {
            _movable.Push(direction);
        }

        public void StopMovement()
        {
            _movable.StopMovement();
        }

        public void StartMovement()
        {
            _movable.StartMovement();
        }

        public void CancelMovement()
        {
            _movable.CancelMovement();
        }

        public bool HasReachedDestination => _movable.HasReachedDestination;

        public void UpdateMover()
        {
            _movable.UpdateMover();
        }

        private void LateUpdate()
        {
            UpdateMover();
        }
    }
}