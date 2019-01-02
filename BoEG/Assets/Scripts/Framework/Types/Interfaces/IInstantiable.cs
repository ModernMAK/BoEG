using System;

namespace Framework.Types
{
    [Obsolete]
    public interface IInstantiable<T> : IInstantiable
    {
        void Instantiate(T data);
    }
    public interface IInstantiable
    {
        void Instantiate();
        void Terminate();
    }
}