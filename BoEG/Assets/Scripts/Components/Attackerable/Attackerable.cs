using System;
using UnityEngine;

namespace Components.Attackerable
{
    [Serializable]
    public class Attackerable : FullModule, IAttackerable
    {
        public Attackerable(IAttackerableData data)
        {
            _data = data;
        }
        
        
        /// <summary>
        /// The backing field containing template data for Attackerable.
        /// </summary>
        [SerializeField] private IAttackerableData _data;


        public float Damage
        {
            get
            {
                var baseVal = _data.BaseDamage;
                var gainRate = _data.GainDamage;
                Func<IAttackerableBuffInstance, float> bonusFunc = (x => x.DamageBonus);
                Func<IAttackerableBuffInstance, float> multFunc = (x => x.DamageMultiplier);
                return this.CalculateValue(baseVal, gainRate, bonusFunc, multFunc, true);
            }
        }

        public float AttackRange
        {
            get
            {
                var baseVal = _data.BaseAttackRange;
                var gainRate = _data.GainAttackRange;
                Func<IAttackerableBuffInstance, float> bonusFunc = (x => x.AttackRangeBonus);
                
                return this.CalculateValueBonus(baseVal, gainRate, bonusFunc, true);
            }
        }

        public float AttackSpeed
        {
            get
            {
                var baseVal = _data.BaseAttackSpeed;
                var gainRate = _data.GainAttackSpeed;
                Func<IAttackerableBuffInstance, float> bonusFunc = (x => x.AttackSpeedBonus);

                return this.CalculateValueBonus(baseVal, gainRate, bonusFunc, true);
            }
        }
    }
}