using System.Collections.Generic;
using Old.Entity.Core;
using Old.Entity.Modules.Abilityable;
using Old.Entity.Modules.Attackerable;
using Old.Entity.Modules.Healthable;
using Old.Entity.Modules.Levelable;
using Old.Entity.Modules.Magicable;
using Old.Entity.Modules.Movable;
using UnityEngine;
using ArmorableData = Modules.Armorable.ArmorableData;
using AttackerableData = Modules.Attackerable.AttackerableData;

namespace Old.Entity
{
//    [CreateAssetMenu(fileName = "Hero.asset", menuName = "Entity/Hero")]
    public class HeroData : DataGroupAsset
    {
        [SerializeField] private string _name;
        
        [SerializeField] private AbilityableData _abilityable;
        [SerializeField] private ArmorableData _armorable;
        [SerializeField] private AttackerableData _attackerable = AttackerableData.Default;
        [SerializeField] private HealthableData _healthable = HealthableData.Default;
        [SerializeField] private LevelableData _levelable;
        [SerializeField] private MagicableData _magicable;
        [SerializeField] private MovableData _movable;
        
        public string Name
        {
            get { return _name; }
        }
        
        protected override IEnumerable<object> Data
        {
            get
            {
                yield return _abilityable;
                yield return _armorable;
                yield return _attackerable;
                yield return _healthable;
                yield return _levelable;
                yield return _magicable;
                yield return _movable;                
            }
        }
    }
}