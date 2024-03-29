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
        IProxy<ITeamable>, IProxy<IMovable>, IProxy<IDamageable>, IProxy<ITargetable>, IProxy<IModifiable>, IProxy<IKillable>,
        IProxy<IHealable>
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
        [SerializeField] private Damageable _damageTarget;
        [SerializeField] private Targetable _targetable;
        [SerializeField] private Modifiable _modifiable;
        [SerializeField] private Killable _killable;
        [SerializeField] private Inventoryable<IItem> _inventoryable;
        [SerializeField] private Healable _healable;
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
        IDamageable IProxy<IDamageable>.Value => _damageTarget;
        ITargetable IProxy<ITargetable>.Value => _targetable;
        IModifiable IProxy<IModifiable>.Value => _modifiable;
        IKillable IProxy<IKillable>.Value => _killable;
        IHealable IProxy<IHealable>.Value => _healable;
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
                yield return _inventoryable;
                yield return _healable;
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
            _damageTarget = new Damageable(this, _healthable, _killable, _armorable);
            _modifiable = new Modifiable(this);
            _inventoryable = new Inventoryable<IItem>(this, new LimitedInventory<IItem>(6));
            _healable = new Healable(this,_healthable);
        }


        public void Initialize(IHeroData data)
        {
            _icon = data.Icon;
            _healthable.Initialize(data.HealthableData);
            _magicable.Initialize(data.MagicableData);
            _armorable.Initialize(data.ArmorableData);
            _attackerable.Initialize(data.AttackerableData);
            _movable.Initialize(data.MovableData);
            _teamable.Initialize(_initialTeam);
            var instanceAbilities = new AbilityObject[data.Abilities.Count];
            for (var i = 0; i < data.Abilities.Count; i++) instanceAbilities[i] = Instantiate(data.Abilities[i]);
            _abilitiable.Initialize(instanceAbilities);

            var modifiableListeners = GetModules<IListener<IModifiable>>();
            foreach(var listener in modifiableListeners)
                listener.Register(_modifiable);
        }

        protected override void SetupComponents()
        {
            base.SetupComponents();
            if (_data != null)
                Initialize(_data);
        }

    }
}