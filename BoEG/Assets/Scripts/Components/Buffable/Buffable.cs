using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Components.Buffable
{
    public class Buffable : Module, IBuffable
    {
        [SerializeField] private List<IBuffInstance> _buffs;

        public Buffable()
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