using System;
using Core;
using UnityEngine;
using UnityEngine.Networking;

namespace Components.Magicable
{
    [Serializable]
    public class MagicableInstance : FullModule, IMagicableInstance
    {
        [SerializeField] [Range(0f, 1f)] private float _manaRatio;
        [SerializeField] private IMagicableData _data;
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
                writer.Write(_manaRatio);


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
                _manaRatio = reader.ReadSingle();
        }

        public MagicableInstance(IMagicableData data)
        {
            _manaRatio = 1f;
            _data = data;
        }

       
        public float ManaPoints
        {
            get { return _manaRatio * ManaCapacity; }
            set { ManaRatio = value / ManaCapacity; }
        }

        public float ManaRatio
        {
            get { return _manaRatio; }
            set
            {
                var temp = Mathf.Clamp01(value);
                if (!_manaRatio.SafeEquals(temp))
                    _dirtyMask |= 0x01;
                _manaRatio = temp;
            }
        }

        public float ManaCapacity
        {
            get
            {
                var baseVal = _data.BaseManaCapacity;
                var gainRate = _data.GainManaGen;
                Func<IMagicableBuffInstance, float> bonusFunc = (x => x.ManaCapacityBonus);
                Func<IMagicableBuffInstance, float> multFunc = (x => x.ManaCapacityMultiplier);
                return this.CalculateValue(baseVal, gainRate, bonusFunc, multFunc, true);
            }
        }

        public float ManaGen
        {
            get
            {
                var baseVal = _data.BaseManaGen;
                var gainRate = _data.GainManaGen;
                Func<IMagicableBuffInstance, float> bonusFunc = (x => x.ManaGenBonus);
                Func<IMagicableBuffInstance, float> multFunc = (x => x.ManaGenMultiplier);

                return this.CalculateValue(baseVal, gainRate, bonusFunc, multFunc);
            }
        }

        public override void PreTick()
        {
            base.PreTick();
            ManaPoints += ManaGen * Time.deltaTime;
        }
    }
}