using System.Collections.Generic;
using Framework.Core;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using UnityEngine;

namespace MobaGame.Entity.UnitArchtypes
{
    public class Core : CommandableActor, IProxy<IHealthable>, IProxy<IArmorable>, IProxy<IDamageTarget>, IInitializable<ICoreData>, IProxy<ITeamable>, IProxy<IKillable>
    {
#pragma warning disable 649

        [Header("Data")] [SerializeField] private CoreData _data;

        [SerializeField] private TeamData _initialTeam;
        [Header("Sub Components")] private Healthable _healthable;
        private Armorable _armorable;
        private DamageTarget _damageTarget;
        private Attackerable _attackerable;
        private Teamable _teamable;
        private Killable _killable;

#pragma warning restore 649
        IHealthable IProxy<IHealthable>.Value => _healthable;

        IArmorable IProxy<IArmorable>.Value => _armorable;

        IDamageTarget IProxy<IDamageTarget>.Value => _damageTarget;


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
            _killable = new Killable(this);
            _armorable = new Armorable(this);
            _healthable = new Healthable(this);
            _teamable = new Teamable(this);
            _attackerable = new Attackerable(this, _teamable);
            _damageTarget = new DamageTarget(this, _healthable, _killable, _armorable);
        }

        public void Initialize(ICoreData data)
        {
            _icon = data.Icon;
            _healthable.Initialize(data.HealthableData);
            _armorable.Initialize(data.ArmorableData);
            _teamable.Initialize(_initialTeam);
        }
    }
}