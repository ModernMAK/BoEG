using System.Collections.Generic;
using Core;
using UnityEditor;
using UnityEngine;

namespace Components.Abilityable
{
    public class Abilityable : Module
    {
        protected Abilityable() : base()
        {
            _abilities = new List<IAbility>();
        }

        public Abilityable(IAbilityableData data) : this()
        {
            foreach (var abiltiy in data.Abilities)
            {
                AddAbility(abiltiy);
            }
        }

        private readonly List<IAbility> _abilities;

        public override void Initialize(Entity e)
        {
            foreach (var ability in _abilities)
            {
                ability.Initialize(e.gameObject);
            }
        }

        private void AddAbility(IAbilityData abilityData)
        {
            var instance = abilityData.CreateInstance();
            _abilities.Add(instance);
        }

        public void Cast(int index)
        {
            _abilities[index].Trigger();
        }

        public override void Tick()
        {
            KeyCode[] codes = {KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R};
            for (var i = 0; i < codes.Length && i < _abilities.Count; i++)
                if (Input.GetKeyDown(codes[i]))
                    Cast(i);
        }


        //
    }
}