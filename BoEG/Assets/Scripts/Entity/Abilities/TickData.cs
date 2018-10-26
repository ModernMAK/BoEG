using System;
using UnityEngine;

namespace Entity.Abilities
{
    [Serializable]
    public struct TickData
    {
        public TickData(int required, float duration)
        {
            _required = required;
            _duration = duration;
        }

        [SerializeField] private int _required;

        public int TicksRequired
        {
            get { return _required; }
        }

        [SerializeField] private float _duration;

        public float Duration
        {
            get { return _duration; }
        }
    }
}