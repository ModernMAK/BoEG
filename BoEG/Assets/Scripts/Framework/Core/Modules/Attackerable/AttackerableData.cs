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
        [SerializeField] private float _attackSpeed;
        [SerializeField] private float _attackInterval;
        [SerializeField] private bool _isRanged;
#pragma warning restore 649

        public float AttackInterval => _attackInterval;

        public float AttackRange => _attackRange;

        public float AttackDamage => _attackDamage;

        public float AttackSpeed => _attackSpeed;

        public bool IsRanged => _isRanged;
    }
}