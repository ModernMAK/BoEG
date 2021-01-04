using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Commands
{
    public class MoveToCommand : MovementCommand
    {
        public MoveToCommand(GameObject entity, Vector3 target) : base(entity)
        {
            Target = target;
        }

        protected override Vector3 Target { get; }

        protected override bool IsDone()
        {
            return Movable.HasReachedDestination;
        }
    }
}