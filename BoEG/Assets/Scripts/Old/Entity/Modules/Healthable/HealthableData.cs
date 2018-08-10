using System;
using UnityEngine;

namespace Old.Entity.Modules.Healthable
{
    [Serializable]
    public struct HealthableData : IHealthableData
    {
        [SerializeField] private float _baseHealthCapacity;
        [SerializeField] private float _baseHealthGen;
        [SerializeField] private float _gainHealthCapacity;
        [SerializeField] private float _gainHealthGen;

        public float BaseHealthCapacity
        {
            get { return _baseHealthCapacity; }
        }

        public float BaseHealthGen
        {
            get { return _baseHealthGen; }
        }

        public float GainHealthCapacity
        {
            get { return _gainHealthCapacity; }
        }

        public float GainHealthGen
        {
            get { return _gainHealthGen; }
        }

        public static HealthableData Default
        {
            get
            {
                return new HealthableData()
                {
                    _baseHealthCapacity = 500f,
                    _baseHealthGen = 2f,
                    _gainHealthCapacity = 0f,
                    _gainHealthGen = 0f
                };
            }
        }
    }
}