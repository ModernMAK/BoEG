//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Entity;
//using UnityEngine;
//
//namespace Modules.Abilityable
//{
//    [Serializable]
//    public class Abilitiable : Module, IAbilitiable
//    {
//        public Abilitiable(GameObject self, IAbilitiableData data) : base(self)
//        {
//            _abilities = new List<IAbility>();
//            foreach (var ability in data.Abilities)
//            {
//                if (ability == null)
//                    continue;
//
//                _abilities.Add(ability.CreateInstance());
//            }
//        }
//
//        public override void Initialize()
//        {
//            foreach (var ability in _abilities)
//            {
//                ability.Initialize(Self);
//            }
//        }
//
//        public override void PreStep(float deltaStep)
//        {
//            _abilities.PreStep(deltaStep);
//        }
//
//        public override void Step(float deltaTick)
//        {
//            _abilities.Step(deltaTick);
//        }
//
//        public override void PostStep(float deltaTick)
//        {
//            _abilities.PostStep(deltaTick);
//        }
//        public override void PhysicsStep(float deltaTick)
//        {
//            _abilities.PhysicsStep(deltaTick);
//        }
//
//        public override void Terminate()
//        {
//            foreach (var ability in _abilities)
//            {
//                ability.Terminate();
//            }
//        }
//
//
//        private readonly List<IAbility> _abilities;
//
//        public T GetAbility<T>() where T : IAbility
//        {
//            foreach (var ability in _abilities)
//            {
//                if (ability is T)
//                    return (T) ability;
//            }
//
//            return default(T);
//        }
//
//        public IEnumerable<IAbility> Abilities
//        {
//            get { return _abilities; }
//        }
//
//        public void Cast(int index)
//        {
//            _abilities[index].Trigger();
//        }
//        public void LevelUp(int index)
//        {
//            _abilities[index].LevelUp();
//        }
//
//    }
//}