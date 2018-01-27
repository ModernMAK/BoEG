using System.Collections.Generic;
using System.Linq;

namespace Entity.Buffable
{
    public class Buffable : UnetBehaviour
    {
        private List<IBuff> _buffs;

        protected override void Awake()
        {
            base.Awake();
            _buffs = new List<IBuff>();
        }

        public void AddBuff(IBuff buff)
        {
            _buffs.Add(buff);
        }

        public bool HasBuff(IBuff buff, bool reference = true)
        {
            return reference ? _buffs.Contains(buff) : _buffs.Any(b => b.GetType() == buff.GetType());
        }

        public void RemoveBuff(IBuff buff, bool reference = true)
        {
            if (reference)
                _buffs.Remove(buff);
            else
                _buffs.RemoveAll(b => b.GetType() == buff.GetType());
        }

        public IEnumerable<T> GetBuffs<T>() where T : IBuff
        {
            //I know LINQ is considered "bad" for performance
            //I also know that until I have something working, I shouldn't worry so much about performance
            return _buffs.OfType<T>();
        }
    }
}