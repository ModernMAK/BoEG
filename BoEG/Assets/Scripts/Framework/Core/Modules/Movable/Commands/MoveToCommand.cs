using UnityEngine;

namespace Framework.Core.Modules.Commands
{
    public class MoveToCommand : MovementCommand
    {
        private readonly Vector3 _target;

        public MoveToCommand(GameObject entity, Vector3 target) : base(entity)
        {
            _target = target;
        }

        protected override Vector3 Target => _target;

        protected override bool IsDone()
        {
            return Movable.HasReachedDestination;
        }
    }
}