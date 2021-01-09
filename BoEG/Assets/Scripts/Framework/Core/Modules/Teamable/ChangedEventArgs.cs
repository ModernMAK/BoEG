using System;

namespace MobaGame.Framework.Core.Modules
{
    public class ChangedEventArgs<T> : EventArgs
    {
        public ChangedEventArgs(T before, T after)
        {
            Before = before;
            After = after;
        }
        public T Before { get; }
        public T After { get; }
    }
}