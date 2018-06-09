using UnityEngine;
using System;
namespace Components.Armorable
{
    /// <summary>
    /// An immutable version of IArmorableData.
    /// </summary>
    [Serializable]
    public struct ArmorableData : IArmorableData
    {
        /// <summary>
        /// Starting value for flat physical damage negation, only editable in the editor.
        /// </summary>
        [SerializeField] private float _basePhysicalBlock;

        /// <summary>
        /// Flat physical damage negation gained from levels, only editable in the editor.
        /// </summary>
        [SerializeField] private float _gainPhysicalBlock;

        /// <summary>
        /// Starting value for percentage-based physical damage negation, only editable in the editor.
        /// </summary>
        [Range(-1f, 1f)] [SerializeField] private float _basePhysicalResist;

        /// <summary>
        /// Percentage-based physical damage negation gained from levels, only editable in the editor.
        /// </summary>
        [Range(-1f, 1f)] [SerializeField] private float _gainPhysicalResist;

        /// <summary>
        /// The backing field for complete negation of physical damage, only editable in the editor.
        /// </summary>
        [SerializeField] private bool _hasPhysicalImmunity;

        /// <summary>
        /// Starting value for flat magical damage negation, only editable in the editor.
        /// </summary>
        [SerializeField] private float _baseMagicalBlock;

        /// <summary>
        /// Flat magical damage negation gained from levels, only editable in the editor.
        /// </summary>
        [SerializeField] private float _gainMagicalBlock;

        /// <summary>
        /// Starting value for Percentage-based magical damage negation, only editable in the editor.
        /// </summary>
        [Range(-1f, 1f)] [SerializeField] private float _baseMagicalResist;

        /// <summary>
        /// Percentage-based magical damage negation gained from levels, only editable in the editor.
        /// </summary>
        [Range(-1f, 1f)] [SerializeField] private float _gainMagicalResist;

        /// <summary>
        /// Complete negation of magical damage, only editable in the editor.
        /// </summary>
        [SerializeField] private bool _hasMagicalImmunity;


        public float BasePhysicalBlock
        {
            get { return _basePhysicalBlock; }
        }

        public float BaseMagicalBlock
        {
            get { return _baseMagicalBlock; }
        }

        public bool HasPhysicalImmunity
        {
            get { return _hasPhysicalImmunity; }
        }

        public float BasePhysicalResist
        {
            get { return _basePhysicalResist; }
        }

        public float BaseMagicalResist
        {
            get { return _baseMagicalResist; }
        }

        public bool HasMagicalImmunity
        {
            get { return _hasMagicalImmunity; }
        }

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