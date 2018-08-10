using UnityEngine;

namespace Old.Entity.Modules.Movable
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
                _movable.Teleport(Destenation);
            }
            if (Pause)
            {
                Pause = !Pause;
                _movable.Pause();
            }
            if (Resume)
            {
                Resume = !Resume;
                _movable.Resume();
            }
            if (Stop)
            {
                Stop = !Stop;
                _movable.Stop();
            }
        }
    }
}