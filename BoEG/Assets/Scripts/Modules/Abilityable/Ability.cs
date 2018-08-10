using UnityEngine;

namespace Modules.Abilityable
{
    public abstract class Ability : ScriptableObject, IAbility, IAbilityData    
    {
        public string Name;

//        public Sprite Icon;
//        public float Cooldown;

//        public int Level;
        public abstract void Initialize(GameObject go);
        public abstract void Terminate();
        
        public abstract void Trigger();

        public virtual IAbility CreateInstance()
        {
            return Instantiate(this);
        }

        public virtual void Tick(float deltaTick)
        {
            
        }
        public virtual void PreTick(float deltaTick)
        {
            
        }
        public virtual void PostTick(float deltaTick)
        {
            
        }
        public virtual void PhysicsTick(float deltaTick)
        {
            
        }
    }
}