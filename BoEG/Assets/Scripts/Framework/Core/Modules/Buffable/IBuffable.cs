using System;
using System.Collections.Generic;
using Old.Modules.Buffable;
using UnityEngine;

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

    public class BuffableComponent : MonoBehaviour, IComponent<IBuffable>, IBuffable
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

    public class Buffable : IBuffable
    {
        public void AddBuff(IBuff buff)
        {
            var args = new BuffEventArgs(buff);
            OnAdding(args);
            //We... don't actually cache the buffs
            OnAdded(args);
        }

        public void RemoveBuff(IBuff buff)
        {
            var args = new BuffEventArgs(buff);
            OnRemoving(args);
            //We... don't actually cache the buffs
            OnRemoved(args);
        }

        public event EventHandler<BuffEventArgs> Adding;
        public event EventHandler<BuffEventArgs> Added;
        public event EventHandler<BuffEventArgs> Removing;
        public event EventHandler<BuffEventArgs> Removed;

        protected virtual void OnAdding(BuffEventArgs e)
        {
            Adding?.Invoke(this, e);
        }

        protected virtual void OnAdded(BuffEventArgs e)
        {
            Added?.Invoke(this, e);
        }

        protected virtual void OnRemoving(BuffEventArgs e)
        {
            Removing?.Invoke(this, e);
        }

        protected virtual void OnRemoved(BuffEventArgs e)
        {
            Removed?.Invoke(this, e);
        }
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