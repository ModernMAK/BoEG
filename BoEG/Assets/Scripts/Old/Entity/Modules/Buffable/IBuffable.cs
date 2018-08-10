using System.Collections.Generic;

namespace Old.Entity.Modules.Buffable
{
    public interface IBuffable
    {
        IEnumerable<T> GetBuffs<T>() where T : IBuffInstance;
        void RegisterBuff(IBuffInstance instance);
    }
}