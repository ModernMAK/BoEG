using UnityEngine;

namespace Framework.Core.Modules
{
    public interface IMovable
    {
        float MoveSpeed { get; }
        float TurnSpeed { get; }
        void MoveTo(Vector3 destenation);
        void WarpTo(Vector3 destenation);
        void Push(Vector3 direction);

        void StopMovement();
        void StartMovement();
        void CancelMovement();
        
        bool HasReachedDestenation { get; }
    }
}