using System;
using Core;
using Old.Entity.Modules.Armorable;
using Old.Util;
using UnityEngine;
using UnityEngine.Networking;
using IArmorable = Modules.Armorable.IArmorable;

namespace Old.Entity.Modules.Healthable
{
    [Serializable]
    [DisallowMultipleComponent]
    public class Healthable : FullModule, IHealthable
    {
        //Variables -----------
        
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] [Range(0f, 1f)] private float _healthRatio = 1f;
        [SerializeField] private IHealthableData _data;
        private IArmorable _armorable;
        
        //Module Functions -----------

        protected override void Initialize()
        {
            _armorable = GetComponent<IArmorable>();
            _data = GetData<IHealthableData>();
            
        }


        
        protected override void PreTick()
        {
            base.PreTick();
            ModifyHealth(HealthGen * Time.deltaTime);
        }

        protected override void PostTick()
        {
            base.PostTick();
            if (_healthRatio.SafeEquals(0f))
                Die();
        }

        protected override bool Serialize(NetworkWriter writer, bool initState)
        {
            var mask = syncVarDirtyBits;
            
            if (!initState)
                writer.Write(mask);
            else
                mask = byte.MaxValue;
            
            if (mask.HasBit(0))
                writer.Write(_healthRatio);


            return (mask != 0);
        }

        protected override void Deserialize(NetworkReader reader, bool initState)
        {
            var mask = byte.MaxValue;
            
            if (!initState)
                mask = reader.ReadByte();
            
            if (mask.HasBit(0))
                _healthRatio = reader.ReadSingle();
        }
        
        //Healthable Functions -----------

        public float HealthPoints
        {
            get { return _healthRatio * HealthCapacity; }
            private set { HealthRatio = value / HealthCapacity; }
        }

        public float HealthRatio
        {
            get { return _healthRatio; }
            private set
            {
                var temp = Mathf.Clamp01(value);
                if (!_healthRatio.SafeEquals(temp))
                    SetDirtyBit(0x01);
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

        public void TakeDamage(Damage damage)
        {
            damage = (_armorable != null ? _armorable.ResistDamage(damage) : damage);
            ModifyHealth(-damage.Value);
            OnDamageTaken(new DamageEventArgs(damage));
            if (_healthRatio.SafeEquals(0f))
            {
                var me = damage.Source.GetComponent<MiscEventable>();
                if(me != null)
                    me.OnKilledEntity(new KillEventArgs(damage.Source,gameObject));
                Die();                
            }
        }
        public void ModifyHealth(float modified)
        {
            HealthPoints += modified;
            OnMoidifyHealth(new HealthModifiedEventArgs(modified));
        }

        public void Die()
        {
            gameObject.SetActive(false);
        }

        public event HealthModifiedHandler HealthModified;
        public event DamageHandler DamageTaken;

        private void OnDamageTaken(DamageEventArgs damageArgs)
        {
            if (DamageTaken != null)
                DamageTaken(damageArgs);
        }
        private void OnMoidifyHealth(HealthModifiedEventArgs healthArgs)
        {
            if (HealthModified != null)
                HealthModified(healthArgs);
        }

    }
}