using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class BuffableComponent : MonoBehaviour, IInitializable<IBuffable>, IBuffable
    {
        private IBuffable _buffable;
        public void AddBuff(IBuff buff)
        {
            _buffable.AddBuff(buff);
        }

        public void RemoveBuff(IBuff buff)
        {
            _buffable.RemoveBuff(buff);
        }

        public event EventHandler<BuffEventArgs> Adding
        {
            add => _buffable.Adding += value;
            remove => _buffable.Adding -= value;
        }

        public event EventHandler<BuffEventArgs> Added
        {
            add => _buffable.Added += value;
            remove => _buffable.Added -= value;
        }

        public event EventHandler<BuffEventArgs> Removing
        {
            add => _buffable.Removing += value;
            remove => _buffable.Removing -= value;
        }

        public event EventHandler<BuffEventArgs> Removed
        {
            add => _buffable.Removed += value;
            remove => _buffable.Removed -= value;
        }

        public void Initialize(IBuffable module)
        {
            _buffable = module;
        }
    }
}