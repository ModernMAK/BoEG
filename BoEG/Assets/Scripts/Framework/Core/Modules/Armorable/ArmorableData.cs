using System;
using UnityEngine;

namespace Framework.Core.Modules
{
    [Serializable]
    public struct ArmorableData : IArmorableData
    {
#pragma warning disable 649
        [SerializeField] private ArmorData _physical;
        [SerializeField] private ArmorData _magical;
#pragma warning restore 649

        public ArmorData Physical => _physical;

        public ArmorData Magical => _magical;
    }
}