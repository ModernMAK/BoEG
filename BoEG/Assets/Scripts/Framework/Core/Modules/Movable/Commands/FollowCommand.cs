using UnityEngine;

namespace Framework.Core.Modules.Commands
{
    public class FollowCommand : MovementCommand
    {
        private readonly Transform _target;
        private readonly Transform _self;

        public FollowCommand(IMovable movable, Transform self, Transform target) : base(movable)
        {
            _self = self;
            _target = target;
        }


        private Vector3 Direction
        {
            get { return (_target.position - _self.position); }
        }

        private Vector3 Location
        {
            get { return _target.position - Direction.normalized * 1f; }
        }

        protected override Vector3 Target
        {
            get { return Location; }
        }

        protected override bool IsDone()
        {
            return false;
        }
    }
}