using System;
using Framework.Utility;
using UnityEngine;

namespace Framework.Ability
{
    public class TickInstance
    {
        private readonly float _interval;
        private readonly Action<int> _logic;
        private readonly int _ticks;
        private int _ticksPerformed;
        private float _timeAdvanced;

        public TickInstance(float interval, int ticks, Action<int> tickAction)
        {
            if (interval.SafeEquals(0f))
                throw new ArgumentException("interval cannot be 0f");
            if (ticks == 0)
                throw new ArgumentException("ticks cannot be 0");
            _interval = interval;
            _ticks = ticks;
            _ticksPerformed = 0;
            _timeAdvanced = 0f;
            _logic = tickAction;
        }

        private int TicksLeft => _ticks - _ticksPerformed;

        private int TicksToPerform
        {
            get
            {
                var ticks = Mathf.FloorToInt(_timeAdvanced / _interval);
                return Mathf.Min(ticks, TicksLeft);
            }
        }

        public bool IsDone => _ticks <= _ticksPerformed;

        public void Advance(float deltaTime)
        {
            _timeAdvanced += deltaTime;
        }

        public void Perform()
        {
            var performing = TicksToPerform;
            for (var i = 0; i < performing; i++)
                _logic(_ticksPerformed + i);
            _ticksPerformed += performing;
        }
    }
}