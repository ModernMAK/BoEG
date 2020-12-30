using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    [Serializable]
    public struct AttackerableData : IAttackerableData
    {
#pragma warning disable 649
        [SerializeField] private float _attackRange;
        [SerializeField] private float _attackDamage;
        [Tooltip("# of Attacks in an Attack Period")]
        [SerializeField] private float _attackSpeed;
        [Tooltip("Time between Attack Periods")]
        [SerializeField] private float _attackInterval;
        [SerializeField] private bool _isRanged;
#pragma warning restore 649

        public float AttackInterval => _attackInterval;

        public float AttackRange => _attackRange;

        public float AttackDamage => _attackDamage;

        public float AttackSpeed => _attackSpeed;

        public bool IsRanged => _isRanged;

        public static AttackerableData Default => new AttackerableData()
        {
            _attackDamage = 10f,
            _attackSpeed = 1f,
            _attackInterval = 1f,
            _attackRange = 4f,
            _isRanged = true,
        };
    }
}