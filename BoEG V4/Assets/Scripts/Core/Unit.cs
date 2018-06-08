using System.Collections.Generic;
using Components;
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
    public class Player : ScriptableObject
    {
    }


    [RequireComponent(typeof(NavMeshAgent))]
    public class Unit : Entity, IArmorableInstance, IAttackerableInstance, IBuffableInstance, IHealthableInstance,
        ILevelableInstance, IMagicableInstance, IMovableInstance, ITeamableInstance, IControllerableInstance
    {
        [SerializeField] private UnitData _data;
        [SerializeField] private ArmorableInstance _armorable;
        [SerializeField] private AttackerableInstance _attackerable;
        [SerializeField] private BuffableInstance _buffable;
        [SerializeField] private HealthableInstance _healthable;
        [SerializeField] private LevelableInstance _levelable;
        [SerializeField] private MagicableInstance _magicable;
        [SerializeField] private MovableInstance _movable;
        [SerializeField] private TeamableInstance _teamable;
        [SerializeField] private ControllerableInstance _controllerable;

        protected override void Setup()
        {
            _armorable = new ArmorableInstance(_data);
            _attackerable = new AttackerableInstance(_data);
            _buffable = new BuffableInstance();
            _healthable = new HealthableInstance(_data);
            _levelable = new LevelableInstance(_data);
            _magicable = new MagicableInstance(_data);
            _movable = new MovableInstance(_data);
        }

        protected override IEnumerable<IModule> Modules
        {
            get
            {
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
    }
}