using Framework.Types;
using UnityEngine;

namespace Framework.Core.Modules.Commands
{
    public abstract class SimpleMovementCommand : MovementCommand, ICommand
    {
        protected abstract Vector3 Target { get; }

        protected SimpleMovementCommand(IMovable movable) : base(movable)
        {
        }

        protected override void PreStep(float delta)
        {
            Movable.MoveTo(Target);
        }

        protected override void Start()
        {
            Movable.StartMovement();
        }

        protected override void Stop()
        {
            Movable.StopMovement();
        }

    }
}