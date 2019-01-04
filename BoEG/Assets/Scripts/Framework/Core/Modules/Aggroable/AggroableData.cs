using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    [Serializable]
    public struct AggroableData : IAggroableData
    {
        [SerializeField] private float _aggroRange;

        public float AggroRange
        {
            get { return _aggroRange; }
        }
    }
}