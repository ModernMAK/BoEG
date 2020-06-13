using Framework.Core.Modules;
using UnityEngine;

namespace Entity.Abilities.FlameWitch
{
    public class ManaHelper
    {
        public IMagicable Magicable { get; set; }
        
        public bool CanSpendMana(float mana)
        {
            if (Magicable == null)
                return false;
            return (Magicable.Magic > mana);
        }

        public void SpendMana(float mana)
        {
            Magicable.Magic -= mana;
        }
    }
    
    
    public class CooldownHelper
    {
        public float Elapsed { get; set; }
        public float Cooldown { get; set; }

        public float Normal => Mathf.Clamp01(Elapsed / Cooldown);
        public bool Done => Cooldown <= Elapsed;

        public void Reset() => Elapsed = 0f;
        public void Advance(float delta) => Elapsed += delta;
    }
}