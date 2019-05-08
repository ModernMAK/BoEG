using System;
using System.Collections.Generic;

namespace Framework.Core.Modules
{
    public interface IBuffList<T> : IList<T>
    {
        void Subscribe(IBuffable buffable);
        void Unsubscribe(IBuffable buffable);

        event EventHandler<T> Added;
        event EventHandler<T> Removed;
    }
}