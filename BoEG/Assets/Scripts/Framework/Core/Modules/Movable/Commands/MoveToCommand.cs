using UnityEngine;

namespace Framework.Core.Modules.Commands
{
    public class MoveToCommand : SimpleMovementCommand
    {
        private readonly Vector3 _target;

        public MoveToCommand(IMovable movable, Vector3 target) : base(movable)
        {
            _target = target;
        }

        protected override Vector3 Target
        {
            get { return _target; }
        }

        protected override bool IsDone()
        {
            return Movable.HasReachedDestenation;
        }
    }
}