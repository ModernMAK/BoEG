using System;
using UnityEngine;

namespace Framework.Types
{
    [Serializable]
    public struct FloatBuff
    {
        public FloatBuff(float bonus, float multiplier)
        {
            _bonus = bonus;
            _multiplier = multiplier;
        }

        [SerializeField] private float _bonus;
        [SerializeField] private float _multiplier;

        public float Bonus
        {
            get { return _bonus; }
        }

        public float Multiplier
        {
            get { return _multiplier; }
        }
    }
}