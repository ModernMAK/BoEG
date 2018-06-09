using UnityEngine;

namespace Components.Armorable
{
    /// <summary>
    /// A Unity-Asset version of IArmorableData.
    /// </summary>
    [CreateAssetMenu(fileName = "Armorable.asset", menuName = "Component/Armorable")]
    public class ArmorableAsset : ScriptableObject, IArmorableData
    {
        /// <summary>
        /// The backing field for the data, only editable within the editor.
        /// </summary>
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

        public bool HasPhysicalImmunity
        {
            get { return _data.HasPhysicalImmunity; }
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

        public bool HasMagicalImmunity
        {
            get { return _data.HasMagicalImmunity; }
        }
    }
}