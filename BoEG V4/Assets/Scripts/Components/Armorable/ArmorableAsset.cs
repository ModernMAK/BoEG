using UnityEngine;

namespace Components.Armorable
{
    [CreateAssetMenu(menuName = "Component/Armorable")]
    public class ArmorableAsset : ScriptableObject, IArmorableData
    {
        [SerializeField] private ArmorableData _data;

        public float BasePhysicalBlock
        {
            get { return _data.BasePhysicalBlock; }
        }

        public float GainPhysicalBlock
        {
            get { return _data.GainPhysicalBlock; }
        }

        public float BasePhysicalResist
        {
            get { return _data.BasePhysicalResist; }
        }

        public float GainPhysicalResist
        {
            get { return _data.GainPhysicalResist; }
        }

        public bool BasePhysicalImmunity
        {
            get { return _data.BasePhysicalImmunity; }
        }

        public float BaseMagicalBlock
        {
            get { return _data.BaseMagicalBlock; }
        }

        public float GainMagicalBlock
        {
            get { return _data.GainMagicalBlock; }
        }

        public float BaseMagicalResist
        {
            get { return _data.BaseMagicalResist; }
        }

        public float GainMagicalResist
        {
            get { return _data.GainMagicalResist; }
        }

        public bool BaseMagicalImmunity
        {
            get { return _data.BaseMagicalImmunity; }
        }
    }
}