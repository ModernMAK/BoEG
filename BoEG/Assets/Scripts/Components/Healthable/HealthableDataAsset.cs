using UnityEngine;

namespace Components.Healthable
{
    public class HealthableDataAsset : ScriptableObject, IHealthableData
    {
        [SerializeField] private HealthableData _data;

        public float BaseHealthCapacity
        {
            get { return _data.BaseHealthCapacity; }
        }

        public float BaseHealthGen
        {
            get { return _data.BaseHealthGen; }
        }

        public float GainHealthCapacity
        {
            get { return _data.GainHealthCapacity; }
        }

        public float GainHealthGen
        {
            get { return _data.GainHealthGen; }
        }
    }
}