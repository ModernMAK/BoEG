using System;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    [Serializable]
	public struct FloatModifierMultiplier
	{
        public FloatModifierMultiplier(float add, float mul)
		{
            _additive = add;
            _multiplicative = mul;
		}
        [SerializeField]
        private float _additive;
        [SerializeField]
        private float _multiplicative;

        public float Additive => _additive;
        public float Multiplicative => _multiplicative;
        public float Calculate(float value) => value * (Additive + Multiplicative);
        public FloatModifierMultiplier AddModifier(FloatModifierMultiplier modifier) => new FloatModifierMultiplier(Additive + modifier.Additive, (1 + Multiplicative) * (1 + modifier.Multiplicative) - 1);
    }

}