// using System;
// using Framework.Types;
// using Modules.Abilityable;
// using Modules.Attackerable;
// using Modules.Healthable;
// using Modules.Teamable;
// using UnityEngine;
// using Util;
//
// namespace Entity.Abilities.DarkHeart
// {
//     [CreateAssetMenu(fileName = "DarkHeart_Fearmonger.asset", menuName = "Ability/DarkHeart/Fearmonger")]
//     public class Fearmonger : Ability
//     {
//         private Nightmare _nightmareAbility;
//         private IAttackable _attackable;
//         private IHealthable _healthable;
//         private ITeamable _teamable;
//         private DynamicProc _proc;
//
//
//         [SerializeField] [Range(0f, 1f)] private float _minChance;
//         [SerializeField] [Range(0f, 1f)] private float _maxChance;
//         [SerializeField] [Range(0f, 1f)] private float _selfHealthWeight;
//
//         protected override int MaxLevel
//         {
//             get { return 1; }
//         }
//
//         protected override void Initialize()
//         {
//             Level = 1;//This ability is 'innate' and starts off being leveled
//             _nightmareAbility = Self.GetComponent<IAbilitiable>().GetAbility<Nightmare>();
//             _attackable = Self.GetComponent<IAttackable>();
//             _teamable = Self.GetComponent<ITeamable>();
//             _healthable = Self.GetComponent<IHealthable>();
//             _proc = new DynamicProc(0f);
//
//             _attackable.IncomingAttackLanded += IncomingAttackLanded;
//         }
//
//         private float CalculateChance(IHealthable self, IHealthable target)
//         {
//             return self.Normal * _selfHealthWeight +
//                    target.Normal * (1f - _selfHealthWeight);
//         }
//
//         private void Apply(GameObject go, IHealthable healthable)
//         {
//             if (healthable == null)
//                 throw new Exception();
//
//             if (_proc.Proc(CalculateChance(_healthable, healthable)))
//                 _nightmareAbility.UnitCast(go);
//         }
//
//
//         private void IncomingAttackLanded(DamageEventArgs args)
//         {
//             var target = args.Source;
//             var healthable = target.GetComponent<IHealthable>();
//
//             Apply(target, healthable);
//         }
//
//         public override void Terminate()
//         {
//             _attackable.IncomingAttackLanded -= IncomingAttackLanded;
//         }
//
//
//     }
// }

using System;
using Framework.Core;
using Framework.Core.Modules;
using Framework.Types;
using Modules.Teamable;
using UnityEngine;

public interface IAbility
{
    void UnitCast(Actor actor);
}

public interface IAttackable
{
    event EventHandler<DamageEventArgs> IncomingAttackLanded;
}
public interface IAbilitiable
{
    T GetAbility<T>() where T : IAbility;
}

namespace Framework.Heroes.DarkHeart
{
    public class Nightmare : IAbility
    {
        public void UnitCast(Actor actor)
        {
            throw new NotImplementedException();
        }
    }

    public class Fearmonger : IAbility
    {
        private Nightmare _nightmareAbility;
        private IAttackable _attackable;
        private IHealthable _healthable;
        // private ITeamable _teamable;
        private DynamicProc _proc;


        [SerializeField] [Range(0f, 1f)] private float _minChance;
        [SerializeField] [Range(0f, 1f)] private float _maxChance;
        [SerializeField] [Range(0f, 1f)] private float _selfHealthWeight;

        protected void Initialize(Actor actor)
        {
            // Level = 1;//This ability is 'innate' and starts off being leveled
            _nightmareAbility = actor.GetComponent<IAbilitiable>().GetAbility<Nightmare>();
            _attackable = actor.GetComponent<IAttackable>();
            // _teamable = actor.GetComponent<ITeamable>();
            _healthable = actor.GetComponent<IHealthable>();
             _proc = new DynamicProc(0f);
            //
            _attackable.IncomingAttackLanded += IncomingAttackLanded;
        }

        private float CalculateChance(IHealthable self, IHealthable target)
        {
            return self.HealthPercentage * _selfHealthWeight +
                   target.HealthPercentage * (1f - _selfHealthWeight);
        }

        private void Apply(Actor actor)
        {
            var healthable = actor.GetComponent<IHealthable>();
            
            if (_proc.Proc(CalculateChance(_healthable, healthable)))
                _nightmareAbility.UnitCast(actor);
        }


        private void IncomingAttackLanded(object sender, DamageEventArgs args)
        {
            var actor = sender as Actor;
            var target = actor;

            Apply(target);
        }

        public void Terminate()
        {
            _attackable.IncomingAttackLanded -= IncomingAttackLanded;
        }

        public void UnitCast(Actor actor)
        {
            throw new NotImplementedException();
        }
    }
    
    
     public class Necromancy : IAbility
     {
         //Necromancy Chance
         //Skeleton Data
         [SerializeField] private GameObject _skeletonPrefab = null;
         [SerializeField][Range(0f,1f)] private float _skeletonSpawnChance = 0.1f;
         private RandomProc _necromancyProc = null;
         private ITeamable _teamable;
         private IMiscEvent _eventable;
         protected override void Initialize()
         {
             _necromancyProc = new GradualProc(_skeletonSpawnChance);
             _eventable = Self.GetComponent<IMiscEvent>();
             _teamable = Self.GetComponent<ITeamable>();
             if (_eventable == null)
                 throw new MissingModuleException();
             _eventable.KilledEntity += KilledEntityCallback;
         }
         public override void Terminate()
         {
             _eventable.KilledEntity -= KilledEntityCallback;
         }

         private void KilledEntityCallback(KillEventArgs args)
         {
             //Should we check for errors in the KillEventArgs?
             //No, we should assume it is working and check for errors if something is wrong
             if (_necromancyProc.Proc())
             {
                 var location = args.Target.transform.position;

                 var spawned = AbiltiyUtility.Spawn(location, _skeletonPrefab, 1);
                 foreach (var spawn in spawned)
                 {
                     var teamable = spawn.GetComponent<ITeamable>();
                     teamable.SetTeam(_teamable.Team);
                 }
             }
         }
}