using System;
using UnityEngine;

namespace Modules.Armorable
{
    [Serializable]
    public struct Armor
    {
        public Armor(float block, float resistance, bool immunity)
        {
            _block = block;
            _resistance = resistance;
            _immunity = immunity;
        }

        [SerializeField] private float _block;
        [SerializeField] private float _resistance;
        [SerializeField] private bool _immunity;

        public float Block
        {
            get { return _block; }
        }

        public float Resistance
        {
            get { return _resistance; }
        }

        private bool Immune
        {
            get { return _immunity; }
        }

        /// <summary>
        /// Returns the value after reductions
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public float CalculateReduction(float value)
        {
            if (Immune)
                return value;
            //(AttackDamage - Block) * (1 - Resistance)
            //Value clamped after applying reduction to ensure its >= 0
            return Mathf.Max((value - Block) * (1f - Resistance), 0f);
        }
    }
}