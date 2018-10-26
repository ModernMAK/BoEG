using System;

namespace Networking
{
    [Flags]
    public enum PacketType : byte
    {
        Ordered = (1 << 0),
        Reliable = (1 << 1),
        Fragmented = (1 << 2)
    }
}