using UnityEngine;

namespace Framework.Core.Modules
{
    [AddComponentMenu("EndGame/Components/Commandable")]
    [DisallowMultipleComponent]
    public class CommandableComponent : MonoBehaviour, ICommandable, IInitializable<ICommandable>
    {
        private ICommandable _commandable;

        public void PreStep(float deltaTime)
        {
            _commandable.PreStep(deltaTime);
        }

        public void Step(float deltaTime)
        {
            _commandable.Step(deltaTime);
        }

        public void PostStep(float deltaTime)
        {
            _commandable.PostStep(deltaTime);
        }

        public void PhysicsStep(float deltaTime)
        {
            _commandable.PhysicsStep(deltaTime);
        }

        public void AddCommand(ICommand command)
        {
            _commandable.AddCommand(command);
        }

        public void SetCommand(ICommand command)
        {
            _commandable.SetCommand(command);
        }

        public void InterruptCommand(ICommand command)
        {
            _commandable.InterruptCommand(command);
        }

        public void ClearCommands()
        {
            _commandable.ClearCommands();
        }

        public void Initialize(ICommandable module)
        {
            _commandable = module;
        }
    }
}