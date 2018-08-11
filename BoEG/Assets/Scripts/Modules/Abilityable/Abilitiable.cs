using System;
using System.Collections.Generic;
using Entity;
using UnityEngine;

namespace Modules.Abilityable
{
    [Serializable]
    public class Abilitiable : Module, IAbilitiable
    {
        public Abilitiable(GameObject self, IAbilityableData data) : base(self)
        {
            _abilities = new List<IAbility>();
            foreach (var abiltiy in data.Abilities)
            {
                if (abiltiy == null)
                    continue;

                _abilities.Add(abiltiy.CreateInstance());
            }
        }

        public override void Initialize()
        {
            foreach (var ability in _abilities)
            {
                ability.Initialize(Self);
            }
        }

        public override void PreTick(float deltaTick)
        {
            _abilities.PreTick(deltaTick);
        }

        public override void Tick(float deltaTick)
        {
            _abilities.Tick(deltaTick);
        }

        public override void PostTick(float deltaTick)
        {
            _abilities.PostTick(deltaTick);
        }
        public override void PhysicsTick(float deltaTick)
        {
            _abilities.PhysicsTick(deltaTick);
        }

        public override void Terminate()
        {
            foreach (var ability in _abilities)
            {
                ability.Terminate();
            }
        }


        private readonly List<IAbility> _abilities;

        public T GetAbility<T>() where T : IAbility
        {
            foreach (var ability in _abilities)
            {
                if (ability is T)
                    return (T) ability;
            }

            return default(T);
        }

        public void Cast(int index)
        {
            _abilities[index].Trigger();
        }


        //
    }
}