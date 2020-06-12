using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    [Serializable]
    public struct MagicableData : IMagicableData
    {
        [SerializeField]
        private float _magicGeneration;
        [SerializeField]
        private float _magicCapacity;

        public float MagicGeneration => _magicGeneration;

        public float MagicCapacity => _magicCapacity;

        public float MagicGenerationGain => throw new NotImplementedException();

        public float MagicCapacityGain => throw new NotImplementedException();
    }

}