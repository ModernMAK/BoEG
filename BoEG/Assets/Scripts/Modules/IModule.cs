using Core.Serialization;
using Entity;

namespace Modules
{
    public interface IModule : IStepable, ISerializable, IDeserializable
    {
        void Initialize();
        void Terminate();
//        void PreStep();
//        void Step();
//        void PostStep();
    }
}