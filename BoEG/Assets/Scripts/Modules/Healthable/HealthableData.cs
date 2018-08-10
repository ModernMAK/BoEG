using System;
using Core;
using UnityEngine;

namespace Modules.Healthable
{
    [Serializable]
    public struct HealthableData : IHealthableData
    {
        public static HealthableData Default
        {
            get
            {
                return new HealthableData()
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


        public FloatScalar HealthCapacity
        {
            get { return _capacity; }
        }

        public FloatScalar HealthGeneration
        {
            get { return _generation; }
        }
    }
}