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


        private Vector3 Direction => _target.position - Entity.transform.position;

        private Vector3 Location => _target.position - Direction.normalized * DesiredDistance;

        private float DesiredDistance { get; }

        protected override Vector3 Target => Location;

        protected override bool IsDone()
        {
            return false;
        }
    }
}