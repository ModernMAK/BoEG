using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Commands
{
    public abstract class MovementCommand : GameObjectCommand
    {
        protected MovementCommand(GameObject entity) : base(entity)
        {
            Movable = GetModule<IMovable>();
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