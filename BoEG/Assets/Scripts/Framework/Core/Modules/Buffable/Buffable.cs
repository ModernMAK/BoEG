using System;

namespace Framework.Core.Modules
{
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
}