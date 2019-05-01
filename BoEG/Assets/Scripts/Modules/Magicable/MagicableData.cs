using System;
using Framework.Types;
using UnityEngine;

namespace Modules.Magicable
{
    [Serializable]
    public struct MagicableData : IMagicableData
    {
        public static MagicableData Default
        {
            get
            {
                return new MagicableData()
                {
                    _capacity = new FloatScalar(500f),
                    _generation = new FloatScalar(1f),
                };
            }
        }
        
        [SerializeField]
        private FloatScalar _capacity;
        [SerializeField]
        private FloatScalar _generation;


        public FloatScalar ManaCapacity
        {
            get { return _capacity; }
        }

        public FloatScalar ManaGeneration
        {
            get { return _generation; }
        }
    }
}