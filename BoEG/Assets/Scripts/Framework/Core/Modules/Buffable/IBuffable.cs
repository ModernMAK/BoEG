using System;
using System.Collections.Generic;
using Old.Modules.Buffable;

namespace Framework.Core.Modules
{
    public interface IBuffable
    {
        IEnumerable<T> GetBuffs<T>() where T : IBuff;
        void RegisterBuff(IBuff instance);

        event EventHandler<BuffEventArgs> Adding;
        event EventHandler<BuffEventArgs> Added;
        event EventHandler<BuffEventArgs> Removing;
        event EventHandler<BuffEventArgs> Removed;
    }

    public class BuffEventArgs : EventArgs
    {
        public BuffEventArgs(IBuff buff)
        {
            Buff = buff;
        }

        public IBuff Buff { get; private set; }
    }

    public interface IBuff
    {
    }
}