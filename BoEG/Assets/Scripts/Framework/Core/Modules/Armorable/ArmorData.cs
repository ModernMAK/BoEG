using UnityEngine;

namespace Framework.Core.Modules
{
    public struct ArmorData
    {
        public ArmorData(ArmorData data) : this(data.Block, data.Resist, data.Immune)
        {
        }

        public ArmorData(float block, float resist, bool immune) : this()
        {
            Block = block;
            Resist = resist;
            Immune = immune;
        }

        public float Block { get; private set; }
        public float Resist { get; private set; }
        public bool Immune { get; private set; }


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