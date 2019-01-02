using UnityEngine;

namespace Framework.Core.Modules
{
    [RequireComponent(typeof(IMovable))]
    public class MovableTester : MonoBehaviour
    {
        private IMovable _movable;
        public Vector3 Destenation;
        public bool Move;
        public bool Push;
        public bool Teleport;
        public bool Pause;
        public bool Resume;
        public bool Stop;

        private void Awake()
        {
            _movable = GetComponent<IMovable>();
        }

        private void Update()
        {
            if (_movable == null)
                return;
            if (Move)
            {
                Move = !Move;
                _movable.MoveTo(Destenation);
            }
            if (Push)
            {
                Push = !Push;
                _movable.Push(Destenation);
            }
            if (Teleport)
            {
                Teleport = !Teleport;
                _movable.WarpTo(Destenation);
            }
            if (Pause)
            {
                Pause = !Pause;
                _movable.StopMovement();
            }
            if (Resume)
            {
                Resume = !Resume;
                _movable.StartMovement();
            }
            if (Stop)
            {
                Stop = !Stop;
                _movable.CancelMovement();
            }
        }
    }
}