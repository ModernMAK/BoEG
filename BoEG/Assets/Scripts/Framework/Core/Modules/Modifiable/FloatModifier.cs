using System;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    [Serializable]
	public struct FloatModifier
    {
        public FloatModifier(float bonus, float addMul = default, float mulMul = default) : this(bonus, new FloatModifierMultiplier(addMul,mulMul))        {        }
        public FloatModifier(float bonus, FloatModifierMultiplier multiplier)
		{
            _bonus = bonus;
            _multiplier = multiplier;
		}
        [SerializeField]
        private float _bonus;
        [SerializeField]
        private FloatModifierMultiplier _multiplier;
        public float Bonus => _bonus;

        public FloatModifierMultiplier Multiplier => _multiplier;

        public float Calculate(float value) => Bonus + Multiplier.Calculate(value);

        public FloatModifier AddModifier(FloatModifier modifier) => new FloatModifier(Bonus + modifier.Bonus, Multiplier.AddModifier(modifier.Multiplier));
		
	}

}