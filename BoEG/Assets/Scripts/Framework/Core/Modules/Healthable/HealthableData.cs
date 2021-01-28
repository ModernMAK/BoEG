using System;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    [Serializable]
    public struct HealthableData : IHealthableData
    {
#pragma warning disable 649
        [SerializeField] private float _healthGeneration;
        [SerializeField] private float _healthCapacity;
#pragma warning restore 649

        public float Generation => _healthGeneration;

        public float Capacity => _healthCapacity;

        public float GenerationGain => throw new NotImplementedException();

        public float CapacityGain => throw new NotImplementedException();

        public static HealthableData Default =>
            new HealthableData()
            {
                _healthCapacity = 500f,
                _healthGeneration = 1f
            };
    }
}