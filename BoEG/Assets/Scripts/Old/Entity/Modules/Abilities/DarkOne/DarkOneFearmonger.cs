using Old.Entity.Modules.Abilityable;
using UnityEngine;

namespace Old.Entity.Modules.Abilities.DarkOne
{
    [CreateAssetMenu(menuName = "Entity/Ability/DarkOne/Fearmonger")]
    public class DarkOneFearmonger : Ability
    {
        private DarkOneNightmare _nightmareAbility;

        public override void Initialize(GameObject go)
        {
            _nightmareAbility = go.GetComponent<IAbilityable>().GetAbility<DarkOneNightmare>();
        }

        public override void Trigger()
        {
        }

        //TODO impliment a function to act as an attack reciever (IAttackerable acts as an attack launcher)
        //How would this delegate to Healthable? Or Armorable?
    }
}