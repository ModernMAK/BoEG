using System.Collections.Generic;
using Modules.Abilityable.Ability;
using UnityEngine;

namespace Modules.Abilityable
{
    [System.Serializable]
    public struct AbilitiableData : IAbilitiableData
    {
        [SerializeField] private Ability.Ability[] _abilities;

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