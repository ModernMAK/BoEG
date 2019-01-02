using System;
using Framework.Core.Serialization;
using Framework.Utility;
using UnityEngine;
using Util;

namespace Modules.Magicable
{
    [Serializable]
    public class Magicable : Module, IMagicable
    {
        public Magicable(GameObject self, IMagicableData data) : base(self)
        {
            _data = data;
            _manaPercentage = 1f;
			_mask = 0;
        }

        [SerializeField] [Range(0f, 1f)] private float _manaPercentage;
        private readonly IMagicableData _data;
		
		private byte _mask;
		public override void  Serialize(ISerializer serializer)
		{
			serializer.Write(_mask);		
			if(_mask.HasBit(0))
				serializer.Write(_manaPercentage);
			_mask = 0;
		}
		public override void  Deserialize(IDeserializer deserializer)
		{
			var mask = deserializer.ReadByte();		
			if(mask.HasBit(0))
				_manaPercentage = deserializer.ReadFloat();
		}


        public override void PreStep(float deltaStep)
        {
            if (ManaPercentage > 0f && ManaPercentage < 1f)
                ModifyMana(ManaGeneration * deltaStep, Self);
        }

        public override void PostStep(float deltaTime)
        {
        }


        public float ManaPercentage
        {
            get { return _manaPercentage; }
            private set
            {
                value = Mathf.Clamp01(value);
                if(!value.SafeEquals(_manaPercentage))
                {
                    _mask.SetBit(0);
                }
                _manaPercentage = value;
            }
        }

        public float ManaPoints
        {
            get { return _manaPercentage * ManaCapacity; }
            private set { ManaPercentage = value / ManaCapacity; }
        }

        public float ManaCapacity
        {
            get { return _data.ManaCapacity.Evaluate(); }
        }

        public float ManaGeneration
        {
            get { return _data.ManaGeneration.Evaluate(); }
        }

        public void ModifyMana(float modification, GameObject source)
        {
            ManaPoints += modification;
            var args = new ManaModifiedEventArgs(source, Self, modification);
            OnManaModified(args);
        }

        private void OnManaModified(ManaModifiedEventArgs eventArgs)
        {
            if (ManaModified != null)
                ManaModified(eventArgs);
        }

        public event ManaModifiedHandler ManaModified;
    }
}