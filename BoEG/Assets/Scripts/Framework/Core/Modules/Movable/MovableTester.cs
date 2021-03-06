using Framework.Core;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public class MovableTester : MonoBehaviour
    {
        private IMovable _movable;
        public Vector3 Destenation;
        public bool Move;
        public bool Pause;
        public bool Push;
        public bool Resume;
        public bool Stop;
        public bool Teleport;

        private void Awake()
        {
            _movable = this.GetComponent<Actor>().GetModule<IMovable>();
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