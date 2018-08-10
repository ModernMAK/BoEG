using System.Collections.Generic;
using UnityEngine;

namespace Old.Entity.Modules.Abilityable
{
    [CreateAssetMenu(fileName = "Abilitiable.asset", menuName = "Entity/Module/Abilitiable")]
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