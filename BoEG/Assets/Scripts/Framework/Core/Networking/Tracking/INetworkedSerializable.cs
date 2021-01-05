using MobaGame.Framework.Core.Networking.IO;

namespace MobaGame.Framework.Core.Networking.Tracking
{
    public interface INetworkedSerializable
    {
        bool Serialize(WriteOnlyStream stream);
        bool Deserialize(ReadOnlyStream stream);
    }
}