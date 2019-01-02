using System;

namespace Framework.Ability
{
    public class TickFactory
    {
        public TickFactory()
        {
        }

        public TickFactory(float interval, int ticks, Action<int> logic)
        {
            _interval = interval;
            _ticks = ticks;
            _logic = logic;
        }
        public TickFactory(TickFactory reference) : this(reference._interval,reference._ticks,reference._logic)
        {
        }

        private float _interval;
        private int _ticks;
        private Action<int> _logic;

        public TickFactory SetInterval(float interval)
        {
            _interval = interval;
            return this;
        }

        public TickFactory SetTicks(int ticks)
        {
            _ticks = ticks;
            return this;
        }

        public TickFactory SetLogic(Action<int> logic)
        {
            _logic = logic;
            return this;
        }

        public TickInstance Create()
        {
            return new TickInstance(_interval, _ticks, _logic);
        }
    }
}