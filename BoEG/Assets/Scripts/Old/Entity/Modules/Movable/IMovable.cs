using UnityEngine;

namespace Old.Entity.Modules.Movable
{
    public interface IMovable
    {
        float MoveSpeed { get; }
        float TurnSpeed { get; }
        bool MoveTo(Vector3 destenation);
        void Push(Vector3 direction);
        bool Teleport(Vector3 destenation);
        void Pause();
        void Resume();
        void Stop();
    }
}