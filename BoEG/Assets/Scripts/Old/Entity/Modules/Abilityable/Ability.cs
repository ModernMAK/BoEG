using UnityEngine;

namespace Old.Entity.Modules.Abilityable
{
    public abstract class Ability : ScriptableObject, IAbility, IAbilityData    
    {
        public string Name;

//        public Sprite Icon;
        public float Cooldown;

        public int Level;
        public abstract void Initialize(GameObject go);
        public abstract void Trigger();

        public virtual IAbility CreateInstance()
        {
            return Instantiate(this);
        }
    }
        
    public struct TriggerArgs
    {
        
    }

}