using System;
using Core;
using UnityEngine;

namespace Modules.Attackerable
{
    [Serializable]
    public struct AttackerableData : IAttackerableData
    {
        public static AttackerableData Default
        {
            get
            {
                return new AttackerableData()
                {
                    _attackDamage = new FloatScalar(50f),
                    _attackSpeed = new FloatScalar(1f),
                    _attackRange = 5f,

                    _projectile = null
                };
            }
        }

        [SerializeField] private FloatScalar _attackDamage;

        [SerializeField] private FloatScalar _attackSpeed;

        [SerializeField] private float _attackRange;

        [SerializeField] private GameObject _projectile;


        public FloatScalar AttackDamage
        {
            get { return _attackDamage; }
        }

        public FloatScalar AttackSpeed
        {
            get { return _attackSpeed; }
        }

        public float AttackRange
        {
            get { return _attackRange; }
        }

        public GameObject Projectile
        {
            get { return _projectile; }
        }
    }
}