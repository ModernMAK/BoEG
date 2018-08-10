using System;
using UnityEngine;

namespace Modules.Armorable
{
    [Serializable]
    public struct ArmorableData : IArmorableData
    {
        public static ArmorableData Default
        {
            get
            {
                return new ArmorableData()
                {
                    _physical = ArmorData.Default,
                    _magical = ArmorData.Default,
                };
            }
        }
        [SerializeField] private ArmorData _physical;
        [SerializeField] private ArmorData _magical;

        public ArmorData Physical
        {
            get { return _physical; }
        }

        public ArmorData Magical
        {
            get { return _magical; }
        }
    }
}