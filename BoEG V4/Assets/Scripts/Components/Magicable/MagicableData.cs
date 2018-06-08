using UnityEngine;

namespace Components.Magicable
{
    [System.Serializable]
    public struct MagicableData : IMagicableData
    {
        [SerializeField] private float _baseManaCapacity;
        [SerializeField] private float _baseManaGen;
        [SerializeField] private float _gainManaCapacity;
        [SerializeField] private float _gainManaGen;

        public float BaseManaCapacity
        {
            get { return _baseManaCapacity; }
        }

        public float BaseManaGen
        {
            get { return _baseManaGen; }
        }

        public float GainManaCapacity
        {
            get { return _gainManaCapacity; }
        }

        public float GainManaGen
        {
            get { return _gainManaGen; }
        }
    }
}