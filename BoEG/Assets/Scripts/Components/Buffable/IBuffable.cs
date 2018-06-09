using System.Collections.Generic;

namespace Components.Buffable
{
    public interface IBuffable
    {
        IEnumerable<T> GetBuffs<T>() where T : IBuffInstance;
        void RegisterBuff(IBuffInstance instance);
    }
}