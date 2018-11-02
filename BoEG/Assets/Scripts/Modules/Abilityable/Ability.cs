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

        public virtual void LevelUp()
        {
        }

        public virtual IAbility CreateInstance()
        {
            return Instantiate(this);
        }

        public virtual void Step(float deltaTick)
        {
        }

        public virtual void PreStep(float deltaStep)
        {
        }

        public virtual void PostStep(float deltaTick)
        {
        }

        public virtual void PhysicsStep(float deltaTick)
        {
        }
    }
}