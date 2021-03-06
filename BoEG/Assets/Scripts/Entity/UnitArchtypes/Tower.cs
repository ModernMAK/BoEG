using System.Collections.Generic;
using Framework.Core;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using UnityEngine;

namespace MobaGame.Entity.UnitArchtypes
{
    public class Tower : CommandableActor, IProxy<IHealthable>, IProxy<IArmorable>, IProxy<IDamageable>,
        IProxy<IAttackerable>, IInitializable<IBuildingData>, IProxy<ITeamable>, IProxy<IKillable>
    {
#pragma warning disable 649

        [Header("Data")] [SerializeField] private BuildingData _data;

        [SerializeField] private TeamData _initialTeam;
        [Header("Sub Components")] private Healthable _healthable;
        private Armorable _armorable;
        private Damageable _damageTarget;
        private Attackerable _attackerable;
        private Teamable _teamable;
        private Killable _killable;

#pragma warning restore 649
        IHealthable IProxy<IHealthable>.Value => _healthable;

        IArmorable IProxy<IArmorable>.Value => _armorable;

        IDamageable IProxy<IDamageable>.Value => _damageTarget;

        IAttackerable IProxy<IAttackerable>.Value => _attackerable;

        ITeamable IProxy<ITeamable>.Value => _teamable;

        IKillable IProxy<IKillable>.Value => _killable;

        private Sprite _icon;
        public override Sprite GetIcon() => _icon;

        protected override IEnumerable<object> Modules
        {
            get
            {
                foreach (var m in base.Modules)
                    yield return m;
                yield return _healthable;
                yield return _armorable;
                yield return _damageTarget;
                yield return _teamable;
                yield return _attackerable;
                yield return _killable;

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
            base.CreateComponents();
            _armorable = new Armorable(this);
            _healthable = new Healthable(this);
            _teamable = new Teamable(this);
            _attackerable = new Attackerable(this, _teamable);
            _damageTarget = new Damageable(this, _healthable, _killable, _armorable);
        }

        public void Initialize(IBuildingData data)
        {
            _icon = data.Icon;
            _healthable.Initialize(data.HealthableData);
            _armorable.Initialize(data.ArmorableData);
            _attackerable.Initialize(data.AttackerableData);
            _teamable.Initialize(_initialTeam);
        }
    }
}