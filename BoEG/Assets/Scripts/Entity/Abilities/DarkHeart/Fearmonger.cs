using Modules.Abilityable;
using UnityEngine;

namespace Entity.Abilities.DarkHeart
{
    [CreateAssetMenu(fileName = "DarkHeart_Fearmonger.asset", menuName = "Ability/DarkHeart/Fearmonger")]
    public class Fearmonger : Ability
    {
        private Nightmare _nightmareAbility;

        public override void Initialize(GameObject go)
        {
            _nightmareAbility = go.GetComponent<IAbilitiable>()
                .GetAbility<Nightmare>();
        }

        public override void Terminate()
        {
            
        }

        public override void Trigger()
        {
        }

        //TODO impliment a function to act as an attack reciever (IAttackerable acts as an attack launcher)
        //How would this delegate to Healthable? Or Armorable?
    }
}