using UnityEngine;

namespace Old.Entity.Modules.Attackerable
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

        [SerializeField] private float _gainAttackSpeed;

        public float GainDamage
        {
            get { return _gainDamage; }
        }

        public float GainAttackSpeed
        {
            get { return _gainAttackSpeed; }
        }

        [SerializeField] private GameObject _projectile;

        public GameObject Projectile
        {
            get { return _projectile; }
        }

        public static AttackerableData Default
        {
            get
            {
                return new AttackerableData()
                {
                    _baseAttackRange = 1f,

                    _baseDamage = 50f,
                    _gainDamage = 0f,
                    
                    _baseAttackSpeed = 0.8f,
                    _gainAttackSpeed = 0f,
                    
                    _projectile = null
                };
            }
        }
    }
}