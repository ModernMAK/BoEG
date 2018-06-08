using Core;
using UnityEngine;

namespace Components.Movable
{
    public interface IMovableInstance
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