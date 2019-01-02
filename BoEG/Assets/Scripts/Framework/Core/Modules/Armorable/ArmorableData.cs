using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    [Serializable]
    public struct ArmorableData : IArmorableData
    {
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