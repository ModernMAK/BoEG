using System;

namespace Framework.Types
{
    [Obsolete]
    public interface IInstantiable<T> : IInstantiable
    {
        void Instantiate(T data);
    }
    [Obsolete()]
    public interface IInstantiable
    {
        void Instantiate();
        void Terminate();
    }
}