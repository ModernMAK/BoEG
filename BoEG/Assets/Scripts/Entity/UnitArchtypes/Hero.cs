using Framework.Core;
using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using UnityEngine;

namespace MobaGame.Entity.UnitArchtypes
{
    [RequireComponent(typeof(HealthableModule))]
    [RequireComponent(typeof(MagicableModule))]
    [RequireComponent(typeof(DamageTarget))]
    [RequireComponent(typeof(AttackerableModule))]
    [RequireComponent(typeof(Movable))]
    public sealed class Hero : Actor, IInitializable<IHeroData>,
        IProxy<IAbilitiable>, IProxy<IArmorable>, IProxy<IHealthable>, IProxy<IMagicable>, IProxy<IAttackerable>,
        IProxy<ITeamable>
    {
        private Abilitiable _abilitiable;
        private Attackerable _attackerable;
        private Armorable _armorable;
        private Healthable _healthable;
        private Magicable _magicable;
        private Teamable _teamable;

        IAbilitiable IProxy<IAbilitiable>.Value => _abilitiable;
        IArmorable IProxy<IArmorable>.Value => _armorable;
        IHealthable IProxy<IHealthable>.Value => _healthable;
        IMagicable IProxy<IMagicable>.Value => _magicable;
        IAttackerable IProxy<IAttackerable>.Value => _attackerable;
        ITeamable IProxy<ITeamable>.Value => _teamable;

        protected override void CreateComponents()
        {
            _abilitiable = new Abilitiable(this);
            _armorable = new Armorable(this);
            _healthable = new Healthable(this);
            _magicable = new Magicable(this);
            _teamable = new Teamable(this);
            _attackerable = new Attackerable(this, _teamable);
        }

        private Sprite _icon;


        public void Initialize(IHeroData module)
        {
            _icon = module.Icon;
            _healthable.Initialize(module.HealthableData);
            _magicable.Initialize(module.MagicableData);
            _armorable.Initialize(module.ArmorableData);
            _attackerable.Initialize(module.AttackerableData);

            //Movable is still a component, because it uses the NavMeshAgent,
            if (TryGetInitializable<IMovableData>(out var movable))
                movable.Initialize(module.MovableData);
            else throw new MissingComponentException("IMovable");


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
#pragma warning disable 649

        [SerializeField] private HeroData _data;

#pragma warning restore 649
    }
}