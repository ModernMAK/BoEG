using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    [Serializable]
    public struct MagicableData : IMagicableData
    {
        [SerializeField]
        private float _MagicGeneration;
        [SerializeField]
        private float _MagicCapacity;

        public float MagicGeneration
        {
            get { return _MagicGeneration; }
        }

        public float MagicCapacity
        {
            get { return _MagicCapacity; }
        }
    }

}