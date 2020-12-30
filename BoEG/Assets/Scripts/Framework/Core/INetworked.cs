using System.IO;

namespace Framework.Core.Modules
{
    public interface INetworked
    {
        void Read(BinaryReader reader);
        void Write(BinaryWriter writer);
    }
}