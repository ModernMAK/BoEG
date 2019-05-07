using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    [Serializable]
    public struct ArmorData : IArmorData
    {
        public ArmorData(IArmorData data) : this(data.Block, data.Resist, data.Immune)
        {
        }

        public ArmorData(float block, float resist, bool immune)
        {
            _block = block;
            _resist = resist;
            _immune = immune;
        }

        [SerializeField] private float _block;
        [SerializeField] private float _resist;
        [SerializeField] private bool _immune;

        public float Block
        {
            get => _block;
            private set => _block = value;
        }

        public float Resist
        {
            get => _resist;
            private set => _resist = value;
        }

        public bool Immune
        {
            get => _immune;
            private set => _immune = value;
        }


        public ArmorData SetBlock(float block)
        {
            return new ArmorData(this)
            {
                Block = block
            };
        }

        public ArmorData SetResist(float resist)
        {
            return new ArmorData(this)
            {
                Resist = resist
            };
        }

        public ArmorData SetImmunity(bool immune)
        {
            return new ArmorData(this)
            {
                Immune = immune
            };
        }

        public float CalculateReduction(float value)
        {
            if (Immune)
                return value;
            return Mathf.Min(value, Block + Resist * value);
        }
    }
}