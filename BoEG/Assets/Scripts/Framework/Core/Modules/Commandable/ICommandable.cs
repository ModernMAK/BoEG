using Framework.Types;

namespace Framework.Core.Modules
{
    public interface ICommandable : IStepable
    {
        void AddCommand(ICommand command);
        void SetCommand(ICommand command);

        void InterruptCommand(ICommand command);
        void ClearCommands();
    }
}