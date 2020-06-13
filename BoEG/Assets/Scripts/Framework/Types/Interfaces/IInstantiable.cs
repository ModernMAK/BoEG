using System;

namespace Framework.Types
{
    [Obsolete]
    public interface IInstantiable
    {
        void Instantiate();
        void Terminate();
    }
}