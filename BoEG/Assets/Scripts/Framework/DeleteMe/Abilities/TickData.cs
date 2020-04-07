using System;
using UnityEngine;

namespace Entity.Abilities
{
    [Serializable]
    public struct TickData
    {
        public TickData(float interval, float duration)
        {
            _interval = interval;
            _duration = duration;
        }

        [SerializeField] private float _interval;

        public float Interval
        {
            get { return _interval; }
        }

        [SerializeField] private float _duration;

        public float Duration
        {
            get { return _duration; }
        }
    }
}