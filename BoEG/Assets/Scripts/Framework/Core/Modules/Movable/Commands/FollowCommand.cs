using UnityEngine;

namespace Framework.Core.Modules.Commands
{
    public class FollowCommand : MovementCommand
    {
        private readonly Transform _target;

        public FollowCommand(GameObject entity, Transform target, float followDistance = 1f) : base(entity)
        {
            _target = target;
            DesiredDistance = followDistance;
        }


        private Vector3 Direction
        {
            get { return (_target.position - Entity.transform.position); }
        }

        private Vector3 Location
        {
            get { return _target.position - Direction.normalized * DesiredDistance; }
        }

        private float DesiredDistance { get; }

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