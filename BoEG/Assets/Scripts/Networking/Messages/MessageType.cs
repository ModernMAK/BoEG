using UnityEditor.VersionControl;

namespace Networking.Messages
{
    public enum MessageType : byte //int only used to enforce size restrictions
    {
        Error = 0,
        Connect,
        Disconnect,
        Data
    }
}