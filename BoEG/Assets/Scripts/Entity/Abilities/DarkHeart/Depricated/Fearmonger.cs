// using System;
// using Framework.Ability;
// using Framework.Core.Modules;
// using Framework.Types;
// using Modules.Teamable;
// using UnityEngine;
//
// namespace Entity.Abilities.DarkHeart
// {
//     [CreateAssetMenu(menuName = "Ability/DarkHeart/Fearmonger")]
//     public class Fearmonger : AbilityObject
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
//
