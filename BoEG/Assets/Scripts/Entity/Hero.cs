using System.Collections;
using System.Collections.Generic;
using Core;
using Core.OrderSystem;
using Modules;
using Modules.Abilityable;
using Modules.Armorable;
using Modules.Attackerable;
using Modules.Healthable;
using Modules.Magicable;
using Modules.MiscEvents;
using Modules.Movable;
using Modules.Teamable;
using UnityEngine;

namespace Entity
{
    public class Hero : Entity,
        IAbilitiable, IArmorable, IAttackable, IAttackerable,
        IHealthable, IJobSystem,
        IMagicable, IMiscEvent, IMovable,
        ITeamable
    {
        [SerializeField] private HeroData _data;

        [SerializeField] private Abilitiable _abilitiable;
        [SerializeField] private Armorable _armorable;
        [SerializeField] private Attackable _attackable;
        [SerializeField] private Attackerable _attackerable;
        [SerializeField] private Healthable _healthable;
        [SerializeField] private Magicable _magicable;
        [SerializeField] private Movable _movable;
        [SerializeField] private MiscEvent _events;
        [SerializeField] private JobSystem _jobSystem;
        [SerializeField] private Teamable _teamable;

        protected override void Awake()
        {
            base.Awake();

            gameObject.name = _data.Name + " (Hero)";

            gameObject.GetComponentInChildren<MeshRenderer>().material = _data.Mat;

            _abilitiable = new Abilitiable(gameObject, _data.Abilityable);
            _armorable = new Armorable(_data.Armorable);

            _attackable = new Attackable(gameObject);
            _attackerable = new Attackerable(gameObject, _data.Attackerable);

            _healthable = new Healthable(gameObject, _data.Healthable);

            _magicable = new Magicable(gameObject, _data.Magicable);
            _movable = new Movable(gameObject, _data.Movable);
            _events = new MiscEvent();

            _jobSystem = new JobSystem(gameObject);

            _teamable = new Teamable(gameObject);
        }

        protected override void Update()
        {
            base.Update();
            KeyCode[] codes = {KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R};
            for (var i = 0; i < codes.Length; i++)
                if (Input.GetKeyDown(codes[i]))
                    Cast(i);
        }


        protected override IEnumerable<IModule> Modules
        {
            get
            {
                yield return _abilitiable;
                //yield return _armorable;
                yield return _attackable;
                yield return _attackerable;
                yield return _healthable;
                yield return _magicable;
                yield return _movable;
                //yield return _events;
                yield return _jobSystem;
            }
        }


        #region Healthable

        public float HealthPercentage
        {
            get { return _healthable.HealthPercentage; }
        }

        public float HealthPoints
        {
            get { return _healthable.HealthPoints; }
        }

        public float HealthCapacity
        {
            get { return _healthable.HealthCapacity; }
        }

        public float HealthGeneration
        {
            get { return _healthable.HealthGeneration; }
        }


        public void ModifyHealth(float modification)
        {
            _healthable.ModifyHealth(modification);
        }

        public event DEFAULT_HANDLER HealthModified
        {
            add { _healthable.HealthModified += value; }
            remove { _healthable.HealthModified -= value; }
        }

        public void TakeDamage(Damage damage)
        {
            damage = ResistDamage(damage);
            _healthable.TakeDamage(damage);
        }

        public event DamageEventHandler DamageTaken
        {
            add { _healthable.DamageTaken += value; }
            remove { _healthable.DamageTaken -= value; }
        }

        public void Die()
        {
            _healthable.Die();
        }

        public event DEFAULT_HANDLER Died
        {
            add { _healthable.Died += value; }
            remove { _healthable.Died -= value; }
        }

        #endregion

        #region Armorable

        public Armor Physical
        {
            get { return _armorable.Physical; }
        }

        public Armor Magical
        {
            get { return _armorable.Magical; }
        }

        public Damage CalculateDamageAfterReductions(Damage damage)
        {
            return _armorable.CalculateDamageAfterReductions(damage);
        }

        public Damage ResistDamage(Damage damage)
        {
            return _armorable.ResistDamage(damage);
        }

        public event DEFAULT_HANDLER Resisted
        {
            add { _armorable.Resisted += value; }
            remove { _armorable.Resisted -= value; }
        }

        #endregion

        #region Attackable

        public void PrepareAttack(GameObject attacker)
        {
            _attackable.PrepareAttack(attacker);
        }

        public void RecieveAttack(Damage damage)
        {
            _attackable.RecieveAttack(damage);
            _healthable.TakeDamage(damage);
        }

        public event EndgameEventHandler AttackLaunchedAgainst
        {
            add { _attackable.AttackLaunchedAgainst += value; }
            remove { _attackable.AttackLaunchedAgainst -= value; }
        }

        public event DamageEventHandler IncomingAttackLanded
        {
            add { _attackable.IncomingAttackLanded += value; }
            remove { _attackable.IncomingAttackLanded -= value; }
        }

        #endregion

        #region Attackerable

        public float AttackDamage
        {
            get { return _attackerable.AttackDamage; }
        }

        public float AttackRange
        {
            get { return _attackerable.AttackRange; }
        }

        public float AttackSpeed
        {
            get { return _attackerable.AttackSpeed; }
        }

        public GameObject Projectile
        {
            get { return _attackerable.Projectile; }
        }

        public void Attack(GameObject go)
        {
            _attackerable.Attack(go);
        }


        public event DEFAULT_HANDLER OutgoingAttackLanded
        {
            add { _attackerable.OutgoingAttackLanded += value; }
            remove { _attackerable.OutgoingAttackLanded -= value; }
        }

        public event DEFAULT_HANDLER AttackLaunched
        {
            add { _attackerable.AttackLaunched += value; }
            remove { _attackerable.AttackLaunched -= value; }
        }

        #endregion


        #region MiscEvents

        public event KilledHandler KilledEntity
        {
            add { _events.KilledEntity += value; }
            remove { _events.KilledEntity -= value; }
        }

        public void Kill(GameObject go)
        {
            _events.Kill(go);
        }

        #endregion

        #region Abilitiable

        public void Cast(int index)
        {
            _abilitiable.Cast(index);
        }

        public T GetAbility<T>() where T : IAbility
        {
            return _abilitiable.GetAbility<T>();
        }

        #endregion

        #region Movable

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

        #endregion

        #region Job System

        public void StopJobs()
        {
            _jobSystem.StopJobs();
        }

        public void SetJob(IJob job)
        {
            _jobSystem.SetJob(job);
        }

        public void AddJob(IJob job)
        {
            _jobSystem.AddJob(job);
        }

        #endregion

        #region Magicable

        public float ManaPercentage
        {
            get { return _magicable.ManaPercentage; }
        }

        public float ManaPoints
        {
            get { return _magicable.ManaPoints; }
        }

        public float ManaCapacity
        {
            get { return _magicable.ManaCapacity; }
        }

        public float ManaGeneration
        {
            get { return _magicable.ManaGeneration; }
        }

        public void ModifyMana(float modification, GameObject source = null)
        {
            _magicable.ModifyMana(modification, source);
        }

        public event ManaModifiedHandler ManaModified
        {
            add { _magicable.ManaModified += value; }
            remove { _magicable.ManaModified -= value; }
        }

        #endregion

        #region Teamable

        public TeamData Team
        {
            get { return _teamable.Team; }
        }

        public void SetTeam(TeamData team)
        {
            _teamable.SetTeam(team);
        }

        #endregion
    }
}