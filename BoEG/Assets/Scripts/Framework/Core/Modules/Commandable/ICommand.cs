using MobaGame.Framework.Types;

namespace MobaGame.Framework.Core.Modules
{
    public interface ICommand : IStepable
    {
        void Start();
        void Stop();
        bool IsDone();
    }
}