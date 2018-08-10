using System;
using Core;
using UnityEngine;

namespace Modules.Armorable
{
    [Serializable]
    public struct ArmorData
    {
        public static ArmorData Default
        {
            get
            {
                return new ArmorData()
                {
                    _block = new FloatScalar(5f),
                    _resistance = new FloatScalar(0.05f),
                    _immunity = false
                };
            }
        }

        [SerializeField] private FloatScalar _block;
        [SerializeField] private FloatScalar _resistance;
        [SerializeField] private bool _immunity;

        public FloatScalar Block
        {
            get { return _block; }
        }

        public FloatScalar Resistance
        {
            get { return _resistance; }
        }

        public bool Immune
        {
            get { return _immunity; }
        }

        public Armor Evaluate(int levelDelta = 0)
        {
            return new Armor(Block.Evaluate(levelDelta), Resistance.Evaluate(levelDelta), Immune);
        }
    }
}