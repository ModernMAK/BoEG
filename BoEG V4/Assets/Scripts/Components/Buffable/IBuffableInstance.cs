using System.Collections.Generic;

namespace Components.Buffable
{
    public interface IBuffableInstance
    {
        IEnumerable<T> GetBuffs<T>() where T : IBuffInstance;
        void RegisterBuff(IBuffInstance instance);
    }
}