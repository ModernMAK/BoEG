using System;

namespace Framework.Core.Modules
{
    public class BuffEventArgs : EventArgs
    {
        public BuffEventArgs(IBuff buff)
        {
            Buff = buff;
        }

        public IBuff Buff { get; private set; }
    }
}