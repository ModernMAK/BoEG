using Framework.Types;
using UnityEngine;

namespace Framework.Core.Modules.Commands
{
    public abstract class MovementCommand : EntityCommand
    {
        protected abstract Vector3 Target { get; }
        protected IMovable Movable { get; private set; }

        protected MovementCommand(GameObject entity) : base(entity)
        {
            Movable = GetComponent<IMovable>();
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