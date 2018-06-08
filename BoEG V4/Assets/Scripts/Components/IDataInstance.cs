using System;

namespace Components
{
    [Obsolete]
    public interface IDataInstance<in TData>
    {
        void SetData(TData data);
    }
}