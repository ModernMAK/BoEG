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

        public float HealthGeneration => _healthGeneration;

        public float HealthCapacity => _healthCapacity;

        public float HealthGenerationGain => throw new NotImplementedException();

        public float HealthCapacityGain => throw new NotImplementedException();
    }

}