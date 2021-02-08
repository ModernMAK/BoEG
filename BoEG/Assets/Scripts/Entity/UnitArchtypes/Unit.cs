using System.Collections.Generic;
using Framework.Core;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using UnityEngine;
using UnityEngine.AI;

namespace MobaGame.Entity.UnitArchtypes
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(NavMeshObstacle))]
    public class Unit : CommandableActor, IProxy<IArmorable>, IProxy<IHealthable>, IProxy<IMagicable>,
        IProxy<IAttackerable>, IProxy<ITeamable>, IProxy<IMovable>, IProxy<IDamageable>, IInitializable<IUnitData>,
        IProxy<IAggroable>, IProxy<IKillable>
    {
        private Healthable _healthable;
        private Magicable _magicable;
        private Armorable _armorable;
        private Damageable _damageTarget;
        private Attackerable _attackerable;
        private Aggroable _aggroable;
        private Movable _movable;
        private Teamable _teamable;
        private Killable _killable;

        IArmorable IProxy<IArmorable>.Value => _armorable;
        IHealthable IProxy<IHealthable>.Value => _healthable;
        IMagicable IProxy<IMagicable>.Value => _magicable;
        IAttackerable IProxy<IAttackerable>.Value => _attackerable;
        ITeamable IProxy<ITeamable>.Value => _teamable;
        IMovable IProxy<IMovable>.Value => _movable;
        IDamageable IProxy<IDamageable>.Value => _damageTarget;
        IAggroable IProxy<IAggroable>.Value => _aggroable;
        IKillable IProxy<IKillable>.Value => _killable;

        protected override IEnumerable<object> Modules
        {
            get
            {
                foreach (var m in base.Modules)
                    yield return m;
                yield return _armorable;
                yield return _healthable;
                yield return _magicable;
                yield return _attackerable;
                yield return _teamable;
                yield return _movable;
                yield return _damageTarget;
                yield return _aggroable;
                yield return _killable; 
                
            }
        }
        private Sprite _icon;
        public override Sprite GetIcon() => _icon;

        protected override void CreateComponents()
        {
            base.CreateComponents();
            _healthable = new Healthable(this);
            _magicable = new Magicable(this);
            _armorable = new Armorable(this);
            _killable = new Killable(this);
            _damageTarget = new Damageable(this, _healthable, _killable);
            _teamable = new Teamable(this);
            _attackerable = new Attackerable(this, _teamable);
            _aggroable = new Aggroable(this, _teamable);
            var agent = GetComponent<NavMeshAgent>();
            var obstacle = GetComponent<NavMeshObstacle>();
            _movable = new Movable(this, agent, obstacle);
        }

        protected override void SetupComponents()
        {
            base.SetupComponents();
//            var buffable = new Buffable();
//            GetFrameworkComponent<IBuffable>().Initialize(buffable);

            if (_data != null)
                Initialize(_data);
        }
#pragma warning disable 649

        [SerializeField] private UnitData _data;

#pragma warning restore 649
        public void Initialize(IUnitData data)
        {
            _icon = data.Icon;
            _healthable.Initialize(data.HealthableData);
            _magicable.Initialize(data.MagicableData);
            _armorable.Initialize(data.ArmorableData);
            _aggroable.Initialize(data.AggroableData);
            _attackerable.Initialize(data.AttackerableData);
            _movable.Initialize(data.MovableData);
        }
    }
}