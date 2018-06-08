using UnityEngine;

namespace Components.Armorable
{
    [System.Serializable]
    public struct ArmorableData : IArmorableData
    {
        [SerializeField] private float _basePhysicalBlock;
        [SerializeField] private float _gainPhysicalBlock;

        [Range(-1f,1f)]
        [SerializeField] private float _basePhysicalResist;
        [Range(-1f,1f)]
        [SerializeField] private float _gainPhysicalResist;

        [SerializeField] private bool _basePhysicalImmunity;


        [SerializeField] private float _baseMagicalBlock;
        [SerializeField] private float _gainMagicalBlock;

        [Range(-1f,1f)]
        [SerializeField] private float _baseMagicalResist;
        [Range(-1f,1f)]
        [SerializeField] private float _gainMagicalResist;

        [SerializeField] private bool _baseMagicalImmunity;


        //Core
        public float BasePhysicalBlock
        {
            get { return _basePhysicalBlock; }
        }

        public float BaseMagicalBlock
        {
            get { return _baseMagicalBlock; }
        }

        public bool BasePhysicalImmunity
        {
            get { return _basePhysicalImmunity; }
        }

        public float BasePhysicalResist
        {
            get { return _basePhysicalResist; }
        }

        public float BaseMagicalResist
        {
            get { return _baseMagicalResist; }
        }

        public bool BaseMagicalImmunity
        {
            get { return _baseMagicalImmunity; }
        }

        //Level
        public float GainPhysicalBlock
        {
            get { return _gainPhysicalBlock; }
        }

        public float GainPhysicalResist
        {
            get { return _gainPhysicalResist; }
        }

        public float GainMagicalResist
        {
            get { return _gainMagicalResist; }
        }

        public float GainMagicalBlock
        {
            get { return _gainMagicalBlock; }
        }
        
    }
}