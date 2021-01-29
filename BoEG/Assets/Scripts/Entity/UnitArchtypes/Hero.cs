using System.Collections.Generic;
using Framework.Core;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using UnityEngine;
using UnityEngine.AI;

namespace MobaGame.Entity.UnitArchtypes
{
    public sealed class Hero : CommandableActor, IInitializable<IHeroData>,
        IProxy<IAbilitiable>, IProxy<IArmorable>, IProxy<IHealthable>, IProxy<IMagicable>, IProxy<IAttackerable>,
        IProxy<ITeamable>, IProxy<IMovable>, IProxy<IDamageTarget>, IProxy<ITargetable>, IProxy<IModifiable>, IProxy<IKillable>
    {
#pragma warning disable 649
        private Sprite _icon;
        public override Sprite GetIcon() => _icon;

        [Header("Sub Modules")] [SerializeField]
        private Abilitiable _abilitiable;

        [SerializeField] private Attackerable _attackerable;
        [SerializeField] private Armorable _armorable;
        [SerializeField] private Healthable _healthable;
        [SerializeField] private Magicable _magicable;
        [SerializeField] private Teamable _teamable;
        [SerializeField] private Movable _movable;
        [SerializeField] private DamageTarget _damageTarget;
        [SerializeField] private Targetable _targetable;
        [SerializeField] private Modifiable _modifiable;
        [SerializeField] private Killable _killable;
        [Header("Data")] [SerializeField] private HeroData _data;
        [SerializeField] private TeamData _initialTeam;
#pragma warning restore 649

        IAbilitiable IProxy<IAbilitiable>.Value => _abilitiable;
        IArmorable IProxy<IArmorable>.Value => _armorable;
        IHealthable IProxy<IHealthable>.Value => _healthable;
        IMagicable IProxy<IMagicable>.Value => _magicable;
        IAttackerable IProxy<IAttackerable>.Value => _attackerable;
        ITeamable IProxy<ITeamable>.Value => _teamable;
        IMovable IProxy<IMovable>.Value => _movable;
        IDamageTarget IProxy<IDamageTarget>.Value => _damageTarget;
        ITargetable IProxy<ITargetable>.Value => _targetable;
        IModifiable IProxy<IModifiable>.Value => _modifiable;
        IKillable IProxy<IKillable>.Value => _killable;
        protected override IEnumerable<object> Modules
        {
            get
            {
                foreach (var m in base.Modules)
                    yield return m;
                yield return _abilitiable;
                yield return _healthable;
                yield return _magicable;
                yield return _armorable;
                yield return _damageTarget;
                yield return _attackerable;
                // yield return _aggroable;
                yield return _movable;
                yield return _teamable;
                yield return _modifiable;
                yield return _killable;
            }
        }
        protected override void CreateComponents()
        {
            base.CreateComponents();
            _abilitiable = new Abilitiable(this);
            _armorable = new Armorable(this);
            _healthable = new Healthable(this);
            _magicable = new Magicable(this);
            _teamable = new Teamable(this);
            _attackerable = new Attackerable(this, _teamable);
            var agent = GetComponent<NavMeshAgent>();
            var obstacle = GetComponent<NavMeshObstacle>();
            _movable = new Movable(this, agent, obstacle);
            _targetable = new Targetable(this);
            _killable = new Killable(this);
            _damageTarget = new DamageTarget(this, _healthable, _killable, _armorable);
            _modifiable = new Modifiable(this);
        }


        public void Initialize(IHeroData data)
        {
            _icon = data.Icon;
            _healthable.Initialize(data.HealthableData);
            _healthable.Register(_modifiable);

            _magicable.Initialize(data.MagicableData);
            _armorable.Initialize(data.ArmorableData);
            _attackerable.Initialize(data.AttackerableData);
            _movable.Initialize(data.MovableData);
            _teamable.Initialize(_initialTeam);
            var instanceAbilities = new AbilityObject[data.Abilities.Count];
            for (var i = 0; i < data.Abilities.Count; i++) instanceAbilities[i] = Instantiate(data.Abilities[i]);
            _abilitiable.Initialize(instanceAbilities);
        }

        protected override void SetupComponents()
        {
            base.SetupComponents();
//            var buffable = new Buffable();
//            GetFrameworkComponent<IBuffable>().Initialize(buffable);
            if (_data != null)
                Initialize(_data);
        }

    }
}