using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public interface IMovable
    {
        float MoveSpeed { get; }
        float TurnSpeed { get; }

        bool HasReachedDestination { get; }
        void MoveTo(Vector3 destenation);
        void WarpTo(Vector3 destenation);
        void Push(Vector3 direction);

        void Anchor();
        void UnAnchor();
        
        bool Anchored { get; }
        void StopMovement();
        void StartMovement();
        void CancelMovement();
    }
}