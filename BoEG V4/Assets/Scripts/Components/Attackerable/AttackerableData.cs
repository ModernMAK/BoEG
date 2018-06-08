using UnityEngine;

namespace Components.Attackerable
{
    [System.Serializable]
    public struct AttackerableData : IAttackerableData
    {
        //BASE
        [SerializeField] private float _baseDamage;

        [SerializeField] private float _baseAttackRange;

        [SerializeField] private float _baseAttackSpeed;

        //BASE   
        public float BaseDamage
        {
            get { return _baseDamage; }
        }

        public float BaseAttackRange
        {
            get { return _baseAttackRange; }
        }

        public float BaseAttackSpeed
        {
            get { return _baseAttackSpeed; }
        }

        //LEVEL
        [SerializeField] private float _gainDamage;

        [SerializeField] private float _gainAttackRange;

        [SerializeField] private float _gainAttackSpeed;

        public float GainDamage
        {
            get { return _gainDamage; }
        }

        public float GainAttackRange
        {
            get { return _gainAttackRange; }
        }

        public float GainAttackSpeed
        {
            get { return _gainAttackSpeed; }
        }
    }
}