using Framework.Types;

namespace Framework.Core.Modules
{
    public interface ICommand : IStepable
    {
        
        void Start();
        void Stop();
        bool IsDone();
    }
}