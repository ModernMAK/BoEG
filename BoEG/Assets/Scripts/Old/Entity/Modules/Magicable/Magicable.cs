using System;
using Old.Util;
using UnityEngine;
using UnityEngine.Networking;

namespace Old.Entity.Modules.Magicable
{
    [DisallowMultipleComponent]
    [Serializable]
    public class Magicable : FullModule, IMagicable
    {
        //Variables -----------
        
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] [Range(0f, 1f)] private float _magicRatio = 1f;
        [SerializeField] private IMagicableData _data;
        
        //Module Functions -----------

        protected override void Initialize()
        {
            _data =  GetData<IMagicableData>();
        }


        
        protected override void PreTick()
        {
            base.PreTick();
            ModifyMagic(ManaGen * Time.deltaTime);
        }

        protected override bool Serialize(NetworkWriter writer, bool initState)
        {
            var mask = syncVarDirtyBits;
            
            if (!initState)
                writer.Write(mask);
            else
                mask = byte.MaxValue;
            
            if (mask.HasBit(0))
                writer.Write(_magicRatio);


            return (mask != 0);
        }

        protected override void Deserialize(NetworkReader reader, bool initState)
        {
            var mask = byte.MaxValue;
            
            if (!initState)
                mask = reader.ReadByte();
            
            if (mask.HasBit(0))
                _magicRatio = reader.ReadSingle();
        }
        
        //Magicable Functions -----------

        public float ManaPoints
        {
            get { return _magicRatio * ManaCapacity; }
            private set { ManaRatio = value / ManaCapacity; }
        }

        public float ManaRatio
        {
            get { return _magicRatio; }
            private set
            {
                var temp = Mathf.Clamp01(value);
                if (!_magicRatio.SafeEquals(temp))
                    SetDirtyBit(0x01);
                _magicRatio = temp;
            }
        }

        public float ManaCapacity
        {
            get
            {
                var baseVal = _data.BaseManaCapacity;
                var gainRate = _data.GainManaCapacity;
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
        public void ModifyMagic(float modified)
        {
            ManaPoints += modified;
            OnMoidifyMagic(new ManaModifiedEventArgs(modified));
        }
        

        public event ManaModifiedHandler ManaModified;

        private void OnMoidifyMagic(ManaModifiedEventArgs magicArgs)
        {
            if (ManaModified != null)
                ManaModified(magicArgs);
        }

    }
}