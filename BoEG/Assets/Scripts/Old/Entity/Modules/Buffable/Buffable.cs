using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Old.Entity.Modules.Buffable
{
    [DisallowMultipleComponent]
    public class Buffable : Module, IBuffable
    {
        [SerializeField] private List<IBuffInstance> _buffs;

        protected override void Initialize()
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

        public delegate void DELEGATE();

        public event DELEGATE BuffAdded;
    }
}