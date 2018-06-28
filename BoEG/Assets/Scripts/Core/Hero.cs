using System.Collections.Generic;
using Components;
using Components.Abilityable;
using Components.Armorable;
using Components.Attackerable;
using Components.Buffable;
using Components.Controllable;
using Components.Healthable;
using Components.Levelable;
using Components.Magicable;
using Components.Movable;
using Components.Teamable;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Hero : Entity, IAbilityable, IArmorable, IAttackerable, IBuffable, IHealthable,
        ILevelable, IMagicable, IMovable, ITeamable, IControllerable 
    {
        [SerializeField] private HeroData _data;
        [SerializeField] private Abilityable _abilityable;
        [SerializeField] private Armorable _armorable;
        [SerializeField] private Attackerable _attackerable;
        [SerializeField] private Buffable _buffable;
        [SerializeField] private Healthable _healthable;
        [SerializeField] private Levelable _levelable;
        [SerializeField] private Magicable _magicable;
        [SerializeField] private Movable _movable;
        [SerializeField] private Teamable _teamable;
        [SerializeField] private Controllerable _controllerable;

        protected override void Setup()
        {
            _abilityable = new Abilityable(_data);
            _armorable = new Armorable(_data);
            _attackerable = new Attackerable(_data);
            _buffable = new Buffable();
            _healthable = new Healthable(_data);
            _levelable = new Levelable(_data);
            _magicable = new Magicable(_data);
            _movable = new Movable(_data);
        }

        protected override IEnumerable<IModule> Modules
        {
            get
            {
                yield return _abilityable;
                yield return _healthable;
                yield return _magicable;
                yield return _movable;
                yield return _armorable;
                yield return _teamable;
                yield return _attackerable;
//                yield return Buffable;
                yield return _levelable;
//                yield return Controllerable;
            }
        }


        public float PhysicalBlock
        {
            get { return _armorable.PhysicalBlock; }
        }

        public float PhysicalResist
        {
            get { return _armorable.PhysicalResist; }
        }

        public bool PhysicalImmunity
        {
            get { return _armorable.PhysicalImmunity; }
        }

        public float MagicalBlock
        {
            get { return _armorable.MagicalBlock; }
        }

        public float MagicalResist
        {
            get { return _armorable.MagicalResist; }
        }

        public bool MagicalImmunity
        {
            get { return _armorable.MagicalImmunity; }
        }

        public float CalculateReduction(float damage, DamageType type)
        {
            return _armorable.CalculateReduction(damage, type);
        }

        public float Damage
        {
            get { return _attackerable.Damage; }
        }

        public float AttackRange
        {
            get { return _attackerable.AttackRange; }
        }

        public float AttackSpeed
        {
            get { return _attackerable.AttackSpeed; }
        }

        public IEnumerable<T> GetBuffs<T>() where T : IBuffInstance
        {
            return _buffable.GetBuffs<T>();
        }

        public void RegisterBuff(IBuffInstance instance)
        {
            _buffable.RegisterBuff(instance);
        }

        public float HealthPoints
        {
            get { return _healthable.HealthPoints; }
            set { _healthable.HealthPoints = value; }
        }

        public float HealthRatio
        {
            get { return _healthable.HealthRatio; }
            set { _healthable.HealthRatio = value; }
        }

        public float HealthCapacity
        {
            get { return _healthable.HealthCapacity; }
        }

        public float HealthGen
        {
            get { return _healthable.HealthGen; }
        }

        public float Experience
        {
            get { return _levelable.Experience; }
        }

        public int Level
        {
            get { return _levelable.Level; }
        }

        public float ManaPoints
        {
            get { return _magicable.ManaPoints; }
            set { _magicable.ManaPoints = value; }
        }

        public float ManaRatio
        {
            get { return _magicable.ManaRatio; }
            set { _magicable.ManaRatio = value; }
        }

        public float ManaCapacity
        {
            get { return _magicable.ManaCapacity; }
        }

        public float ManaGen
        {
            get { return _magicable.ManaGen; }
        }

        public float MoveSpeed
        {
            get { return _movable.MoveSpeed; }
        }

        public float TurnSpeed
        {
            get { return _movable.TurnSpeed; }
        }

        public bool MoveTo(Vector3 destenation)
        {
            return _movable.MoveTo(destenation);
        }

        public void Push(Vector3 direction)
        {
            _movable.Push(direction);
        }

        public bool Teleport(Vector3 destenation)
        {
            return _movable.Teleport(destenation);
        }

        public void Pause()
        {
            _movable.Pause();
        }

        public void Resume()
        {
            _movable.Resume();
        }

        public void Stop()
        {
            _movable.Stop();
        }

        public TeamData Team
        {
            get { return _teamable.Team; }
        }

        public void SetTeam(TeamData team)
        {
            _teamable.SetTeam(team);
        }

        public void Cast(int index)
        {
            _abilityable.Cast(index);
        }
    }
}