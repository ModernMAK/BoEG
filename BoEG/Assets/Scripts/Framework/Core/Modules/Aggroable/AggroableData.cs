using System;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    [Serializable]
    public struct AggroableData : IAggroableData
    {
        [SerializeField] private float _aggroRange;

        public float AggroRange => _aggroRange;

        public static AggroableData Default => new AggroableData()
        {
            _aggroRange = 1f
        };
    }
}