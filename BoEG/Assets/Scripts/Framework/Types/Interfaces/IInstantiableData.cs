using System;

namespace Framework.Core.Modules
{
    [Obsolete()]
    public interface IInstantiableData<T>
    {
        T Data { get; }
    }
}