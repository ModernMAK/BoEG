using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Components.Buffable
{
    public class BuffableInstance : Module, IBuffableInstance
    {
        [SerializeField] private List<IBuffInstance> _buffs;

        public BuffableInstance()
        {
            _buffs = new List<IBuffInstance>();
        }
        

        public IEnumerable<T> GetBuffs<T>() where T : IBuffInstance
        {
            return _buffs.OfType<T>();
        }

        public void RegisterBuff(IBuffInstance instance)
        {
            _buffs.Add(instance);
        }
    }
}