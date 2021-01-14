using System.Collections.Generic;
using Framework.Core;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Entity.UnitArchtypes
{
    public class Building : CommandableActor, IProxy<IHealthable>, IProxy<IArmorable>, IProxy<IDamageTarget>,
        IProxy<IAttackerable>, IInitializable<IBuildingData>, IProxy<ITeamable>
    {
#pragma warning disable 649

        [Header("Data")] [SerializeField] private BuildingData _data;

        [SerializeField] private TeamData _initialTeam;
        [Header("Sub Components")] private Healthable _healthable;
        private Armorable _armorable;
        private DamageTarget _damageTarget;
        private Attackerable _attackerable;
        private Teamable _teamable;

#pragma warning restore 649
        IHealthable IProxy<IHealthable>.Value => _healthable;

        IArmorable IProxy<IArmorable>.Value => _armorable;

        IDamageTarget IProxy<IDamageTarget>.Value => _damageTarget;

        IAttackerable IProxy<IAttackerable>.Value => _attackerable;

        ITeamable IProxy<ITeamable>.Value => _teamable;

        private Sprite _icon;
        public override Sprite GetIcon() => _icon;

        protected override IEnumerable<object> Modules
        {
            get
            {
                foreach (var m in base.Modules)
                    yield return m;
                yield return _healthable;
                yield return _attackerable;
            }
        }

        protected override void SetupComponents()
        {
            base.SetupComponents();
            if (_data != null)
                Initialize(_data);
        }

        protected override void CreateComponents()
        {
            _armorable = new Armorable(this);
            _healthable = new Healthable(this);
            _teamable = new Teamable(this);
            _attackerable = new Attackerable(this, _teamable);
            _damageTarget = new DamageTarget(this, _healthable, _armorable);
        }

        public void Initialize(IBuildingData module)
        {
            _icon = module.Icon;
            _healthable.Initialize(module.HealthableData);
            _armorable.Initialize(module.ArmorableData);
            _attackerable.Initialize(module.AttackerableData);
            _teamable.Initialize(_initialTeam);
        }
    }
}