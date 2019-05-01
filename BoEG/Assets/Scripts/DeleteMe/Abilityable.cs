using System.Collections.Generic;
using DeleteMe.Abilities;

namespace DeleteMe
{
    public class Abilityable : IAbilityable
    {
        public void Cast(int index)
        {
            if (_abilities.Count > index && index >= 0)
                _abilities[index].Cast();
        }

        private readonly List<Ability> _abilities;

        public Abilityable()
        {
            _abilities = new List<Ability>();
        }
    }

    public interface IAbilityable
    {
        void Cast(int index);
    }
}