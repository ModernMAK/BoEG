using System;
using System.Collections.Generic;

namespace Framework.Core.Modules
{
    public interface IBuffCalculator
    {
        void Calculate();
        event EventHandler Calculated;
    }

    public abstract class BuffCalculator<T> : IBuffCalculator
    {
        protected BuffCalculator(IBuffList<T> buffList)
        {
            _buffList = buffList;
            Subscribe(buffList);
        }

        private readonly IBuffList<T> _buffList;
        protected IEnumerable<T> Buffs => _buffList;

        private void Subscribe(IBuffList<T> buffList)
        {
            buffList.Added += Calculate;
            buffList.Removed += Calculate;
        }

        private void Unsubscribe(IBuffList<T> buffList)
        {
            buffList.Added -= Calculate;
            buffList.Removed -= Calculate;
        }

        private void Calculate(object sender, T e)
        {
            Calculate();            
        }

        public void Calculate()
        {
            CalculateLogic();
            OnCalculated();
        }
        protected abstract void CalculateLogic();
        public event EventHandler Calculated;

        protected virtual void OnCalculated()
        {
            Calculated?.Invoke(this, EventArgs.Empty);
        }
    }
}