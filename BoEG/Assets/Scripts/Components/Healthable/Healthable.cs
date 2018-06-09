using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Components.Healthable
{
    [Serializable]
    public class Healthable : FullModule, IHealthable
    {
        public Healthable(IHealthableData data)
        {
            _healthRatio = 1f;
            _data = data;
        }

        [SerializeField] [Range(0f, 1f)] private float _healthRatio;
        [SerializeField] private IHealthableData _data;
        private byte _dirtyMask;

        public override bool IsDirty()
        {
            return _dirtyMask != 0;
        }

        public override bool Serialize(NetworkWriter writer, bool initState)
        {
            if (initState)
                _dirtyMask = byte.MaxValue;
            if (!initState)
                writer.Write(_dirtyMask);
            if (_dirtyMask.HasBit(0))
                writer.Write(_healthRatio);


            var wrote = (_dirtyMask != 0);
            _dirtyMask = 0;
            return wrote;
        }

        public override void Deserialize(NetworkReader reader, bool initState)
        {
            var mask = byte.MaxValue;
            if (!initState)
                mask = reader.ReadByte();
            if (mask.HasBit(0))
                _healthRatio = reader.ReadSingle();
        }

        public float HealthPoints
        {
            get { return _healthRatio * HealthCapacity; }
            set { HealthRatio = value / HealthCapacity; }
        }

        public float HealthRatio
        {
            get { return _healthRatio; }
            set
            {
                var temp = Mathf.Clamp01(value);
                if (!_healthRatio.SafeEquals(temp))
                    _dirtyMask |= 0x01;
                _healthRatio = temp;
            }
        }

        public float HealthCapacity
        {
            get
            {
                var baseVal = _data.BaseHealthCapacity;
                var gainRate = _data.GainHealthCapacity;
                Func<IHealthableBuffInstance, float> bonusFunc = (x => x.HealthCapacityBonus);
                Func<IHealthableBuffInstance, float> multFunc = (x => x.HealthCapacityMultiplier);

                return this.CalculateValue(baseVal, gainRate, bonusFunc, multFunc, true);
            }
        }

        public float HealthGen
        {
            get
            {
                var baseVal = _data.BaseHealthGen;
                var gainRate = _data.GainHealthGen;
                Func<IHealthableBuffInstance, float> bonusFunc = (x => x.HealthGenBonus);
                Func<IHealthableBuffInstance, float> multFunc = (x => x.HealthGenMultiplier);

                return this.CalculateValue(baseVal, gainRate, bonusFunc, multFunc);
            }
        }

        public override void PreTick()
        {
            base.PreTick();
            HealthPoints += HealthGen * Time.deltaTime;
        }
    }
}