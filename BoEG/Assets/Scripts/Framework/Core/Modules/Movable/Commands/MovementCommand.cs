using UnityEngine;

namespace Framework.Core.Modules.Commands
{
    public abstract class MovementCommand : EntityCommand
    {
        protected MovementCommand(GameObject entity) : base(entity)
        {
            Movable = GetComponent<IMovable>();
        }

        protected abstract Vector3 Target { get; }
        protected IMovable Movable { get; }

        protected override void PreStep(float deltaTime)
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