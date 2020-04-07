using System;
using System.Collections.Generic;
using Old.Modules.Buffable;

namespace Framework.Core.Modules
{
    public interface IBuffable
    {
//        IEnumerable<T> GetBuffs<T>() where T : IBuff;
//        void RegisterBuff(IBuff instance);

        void AddBuff(IBuff buff);
        void RemoveBuff(IBuff buff);

        event EventHandler<BuffEventArgs> Adding;
        event EventHandler<BuffEventArgs> Added;
        event EventHandler<BuffEventArgs> Removing;
        event EventHandler<BuffEventArgs> Removed;
    }
}