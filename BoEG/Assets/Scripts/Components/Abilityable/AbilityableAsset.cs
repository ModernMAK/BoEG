using System.Collections.Generic;
using UnityEngine;

namespace Components.Abilityable
{
    [CreateAssetMenu(menuName = "Component/Abilityable")]
    public class AbilityableAsset : ScriptableObject, IAbilityableData
    {
        [SerializeField] private AbilityableData _data;

        public IEnumerable<IAbilityData> Abilities
        {
            get { return _data.Abilities; }
        }

        public int AbilityCount
        {
            get { return _data.AbilityCount; }
        }
    }
}