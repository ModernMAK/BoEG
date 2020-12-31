using System;

namespace Framework.Core.Networking
{
    public interface INetworkIdentifier
    {
        public Guid Id { get; }
    }

    public interface INetworkSubIdentifier<out TSub>
    {
        public INetworkIdentifier Parent { get; }
        public TSub Id { get; }
    }
}