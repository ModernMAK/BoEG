//using System.Linq;
//using Framework.Core.Modules;
//using UnityEngine;
//
//namespace Framework.Ability.Hero.WarpedMagi
//{
//    /// <summary>
//    /// [ ] Follows Spell Targetability
//    /// </summary>
//    public class MagicalShield : BetterAbility
//    {
//        protected override void Instantiate()
//        {
//            base.Instantiate();
////            _manacost = new AbilityManacost(this, 0f);
////            _cooldown = new AbilityCooldown(0f);
//        }
//
////        private AbilityManacost _manacost;
////        private AbilityCooldown _cooldown;
//
//
//        GameObject[] TriggerCast(Vector3 point, float radius)
//        {
//            var colliders = Physics.OverlapSphere(point, radius);
//            return colliders.Select((c) => c.gameObject).ToArray();
//        }
//
//        void Apply(GameObject go)
//        {
//            var targetable = go.GetComponent<ITargetable>();
//            var magicable = go.GetComponent<IMagicable>();
//            const float ManaLoss = 50f;
//            //const float ManaAsBlock = 1.25f;
//            
//            
//            if (targetable != null && magicable != null)
//            {
//                magicable.ModifyMana(ManaLoss);
//                
//            }
//        }
//    }
//}