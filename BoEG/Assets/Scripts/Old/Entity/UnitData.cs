using System.Collections.Generic;
using Old.Entity.Core;
using Old.Entity.Modules.Abilityable;
using Old.Entity.Modules.Attackerable;
using Old.Entity.Modules.Healthable;
using Old.Entity.Modules.Levelable;
using Old.Entity.Modules.Magicable;
using Old.Entity.Modules.Movable;
using UnityEngine;
using AttackerableData = Modules.Attackerable.AttackerableData;
using IArmorableData = Modules.Armorable.IArmorableData;
using IAttackerableData = Modules.Attackerable.IAttackerableData;

namespace Old.Entity
{
    [CreateAssetMenu(fileName="Unit.asset",menuName="Entity/Unit")]
    public class UnitData : DataGroupAsset, IDataGroup
    {
        [SerializeField] private AbilityableData _abilityable;
        [SerializeField] private IArmorableData _armorable;
        [SerializeField] private AttackerableData _attackerable;
        [SerializeField] private HealthableData _healthable;
        [SerializeField] private LevelableData _levelable;
        [SerializeField] private MagicableData _magicable;
        [SerializeField] private MovableData _movable;
        
        private IAbilityableData Abilityable
        {
            get { return _abilityable; }
        }
        private IArmorableData Armorable
        {
            get { return _armorable; }
        }
        private IAttackerableData Attackerable
        {
            get { return _attackerable; }
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