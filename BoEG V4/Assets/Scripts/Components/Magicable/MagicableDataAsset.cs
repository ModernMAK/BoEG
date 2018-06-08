using UnityEngine;

namespace Components.Magicable
{
    public class MagicableDataAsset : ScriptableObject, IMagicableData
    {
        [SerializeField] private MagicableData _data;

        public float BaseManaCapacity
        {
            get { return _data.BaseManaCapacity; }
        }

        public float BaseManaGen
        {
            get { return _data.BaseManaGen; }
        }

        public float GainManaCapacity
        {
            get { return _data.GainManaCapacity; }
        }

        public float GainManaGen
        {
            get { return _data.GainManaGen; }
        }
    }
}