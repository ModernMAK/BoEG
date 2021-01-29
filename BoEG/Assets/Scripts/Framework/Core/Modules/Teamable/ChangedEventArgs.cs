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
        /// <summary>
        /// The value before the change.
        /// </summary>
        public T Before { get; }
        /// <summary>
        /// The value after the change.
        /// </summary>
        public T After { get; }
    }
}