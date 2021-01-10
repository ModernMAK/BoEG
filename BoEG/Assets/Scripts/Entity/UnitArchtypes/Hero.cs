using System.Collections.Generic;
using Framework.Core;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using MobaGame.Framework.Types;
using UnityEngine;
using UnityEngine.AI;

namespace MobaGame.Entity.UnitArchtypes
{
    public sealed class Hero : Actor, IInitializable<IHeroData>,
        IProxy<IAbilitiable>, IProxy<IArmorable>, IProxy<IHealthable>, IProxy<IMagicable>, IProxy<IAttackerable>,
        IProxy<ITeamable>, IProxy<IMovable>, IProxy<IDamageTarget>, IRespawnable
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

        protected override IEnumerable<IListener<IStepableEvent>> ChildSteppables
        {
            get
            {
                // yield return _abilitiable;
                yield return _healthable;
                yield return _magicable;
                // yield return _armorable;
                // yield return _damageTarget;
                yield return _attackerable;
                // yield return _aggroable;
                yield return _movable;
                // yield return _teamable;
                
                
            }
        }
        protected override void CreateComponents()
        {
            _abilitiable = new Abilitiable(this);
            _armorable = new Armorable(this);
            _healthable = new Healthable(this);
            _magicable = new Magicable(this);
            _teamable = new Teamable(this);
            _attackerable = new Attackerable(this, _teamable);
            var agent = GetComponent<NavMeshAgent>();
            var obstacle = GetComponent<NavMeshObstacle>();
            _movable = new Movable(this, agent, obstacle);

            _damageTarget = new DamageTarget(this, _healthable, _armorable);
        }


        public void Initialize(IHeroData module)
        {
            _icon = module.Icon;
            _healthable.Initialize(module.HealthableData);
            _magicable.Initialize(module.MagicableData);
            _armorable.Initialize(module.ArmorableData);
            _attackerable.Initialize(module.AttackerableData);
            _movable.Initialize(module.MovableData);
            _teamable.Initialize(_initialTeam);
            var instanceAbilities = new AbilityObject[module.Abilities.Count];
            for (var i = 0; i < module.Abilities.Count; i++) instanceAbilities[i] = Instantiate(module.Abilities[i]);
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

        public void Respawn()
        {
            _attackerable.Respawn();
            _healthable.Respawn();
            _magicable.Respawn();
        }
    }
}