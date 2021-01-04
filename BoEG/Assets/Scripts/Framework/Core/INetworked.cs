using System.IO;

namespace MobaGame.Framework.Core
{
    public interface INetworked
    {
        void Read(BinaryReader reader);
        void Write(BinaryWriter writer);
    }
}