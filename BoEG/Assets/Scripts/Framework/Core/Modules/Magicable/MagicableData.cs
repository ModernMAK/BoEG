using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    [Serializable]
    public struct MagicableData : IMagicableData
    {
#pragma warning disable 649
        [SerializeField] private float _magicGeneration;
        [SerializeField] private float _magicCapacity;
        [SerializeField] private float _magicGenerationGain;
        [SerializeField] private float _magicCapacityGain;
#pragma warning restore 649

        public float MagicGeneration => _magicGeneration;

        public float MagicCapacity => _magicCapacity;

        public float MagicGenerationGain => _magicGenerationGain;

        public float MagicCapacityGain => _magicCapacityGain;

        public static MagicableData Default =>
            new MagicableData()
            {
                _magicCapacity = 500f,
                _magicGeneration = 1f,
                _magicCapacityGain = 0f,
                _magicGenerationGain = 0f,
            };
    }
}