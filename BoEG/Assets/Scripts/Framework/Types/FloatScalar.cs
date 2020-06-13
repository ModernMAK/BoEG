using System;
using UnityEngine;

namespace Framework.Types
{
    [Serializable]
    [Obsolete]
    public struct FloatScalar
    {
        public FloatScalar(float flat, float gain = default)
        {
            _base = flat;
            _gain = gain;
        }

        [SerializeField] private float _base;
        [SerializeField] private float _gain;

        public float Base => _base;

        public float Gain => _gain;

        public float Evaluate(int levelDelta = 0)
        {
            return Base + levelDelta * Gain;
        }
    }
}