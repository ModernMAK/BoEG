using System.Collections.Generic;
using UnityEngine;

namespace Old.Entity.Modules.Abilityable
{
    [System.Serializable]
    public struct AbilityableData : IAbilityableData
    {
        [SerializeField] private Ability[] _abilities;

        public IEnumerable<IAbilityData> Abilities
        {
            get { return _abilities; }
        }

        public int AbilityCount
        {
            get { return _abilities.Length; }
        }
    }
}