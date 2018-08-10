using System.Collections.Generic;
using UnityEngine;

namespace Old.Entity.Modules.Abilityable
{
    public class Abilityable : Module
    {
        protected Abilityable() : base()
        {
        }

        public Abilityable(IAbilityableData data) : this()
        {
        }

        protected override void Initialize()
        {
            _abilities = new List<IAbility>();
            var data = GetData<IAbilityableData>();
            foreach (var abiltiy in data.Abilities)
            {
                var instance = abiltiy.CreateInstance();
                _abilities.Add(instance);
                instance.Initialize(gameObject);
            }
        }

        private List<IAbility> _abilities;
        public T GetAbility<T>() where T : IAbility
        {
            foreach (var ability in _abilities)
            {
                if (ability is T)
                    return (T) ability;                
            }
            return default(T);
        }
        private TriggerArgs GetTriggerArgs()
        {
            return new TriggerArgs();
            
        }
        
        public void Cast(int index)
        {
            _abilities[index].Trigger();
        }

        protected override void Tick()
        {
            KeyCode[] codes = {KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R};
            for (var i = 0; i < codes.Length && i < _abilities.Count; i++)
                if (Input.GetKeyDown(codes[i]))
                    Cast(i);
        }


        //
    }
}