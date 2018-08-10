//using Old.Entity.Modules.Abilitiable;
//using Old.Entity.Modules.Attackerable;
//using Old.Entity.Modules.Healthable;
//using UnityEngine;
//using IAttackerable = Modules.Attackerable.IAttackerable;
//
//namespace Old.Entity.Modules.Abilities
//{
//    [CreateAssetMenu(fileName="ScarFeastAbility.asset",menuName = "Entity/Ability/Scar/Feast")]
//    public class ScarFeast : Ability
//    {
//        private IAttackerable _attackerable;
//        private IHealthable _healthable;
//        public override void Initialize(GameObject go)
//        {
//            _attackerable = go.GetComponent<IAttackerable>();
//            _healthable = go.GetComponent<IHealthable>();
//            _attackerable.AttackKilled += KilledLifesteal;
//        }
//
//        void KilledLifesteal(AttackEventArgs atkArgs)
//        {
//            
//            _healthable.ModifyHealth(0.1f * _healthable.HealthCapacity);
//        }
//
//
//        public override void Trigger()
//        {
//            //Does nothing
//        }
//        
//        
//    }
//
//    
//
//}