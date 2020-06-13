using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    [Serializable]
    public struct HealthableData : IHealthableData
    {
#pragma warning disable 649
        [SerializeField]
        private float _healthGeneration;
        [SerializeField]
        private float _healthCapacity;
#pragma warning restore 649

        public float HealthGeneration => _healthGeneration;

        public float HealthCapacity => _healthCapacity;

        public float HealthGenerationGain => throw new NotImplementedException();

        public float HealthCapacityGain => throw new NotImplementedException();
    }

}