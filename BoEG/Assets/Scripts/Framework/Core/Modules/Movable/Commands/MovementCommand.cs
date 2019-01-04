using Framework.Types;
using UnityEngine;

namespace Framework.Core.Modules.Commands
{
    public abstract class MovementCommand : Command
    {
        protected abstract Vector3 Target { get; }
        protected IMovable Movable { get; private set; }

        protected MovementCommand(IMovable movable)
        {
            Movable = movable;
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