using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    [Serializable]
    public struct HealthableData : IHealthableData
    {
        [SerializeField]
        private float _healthGeneration;
        [SerializeField]
        private float _healthCapacity;

        public float HealthGeneration
        {
            get { return _healthGeneration; }
        }

        public float HealthCapacity
        {
            get { return _healthCapacity; }
        }
    }

}