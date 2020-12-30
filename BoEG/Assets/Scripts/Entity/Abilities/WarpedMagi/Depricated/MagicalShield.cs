//using Core;
//using Framework.Types;
//using Modules.Abilityable;
//using Modules.Magicable;
//using Modules.Healthable;
//using Modules.Teamable;
//using Triggers;
//using UnityEngine;
//using Util;
//
//namespace Entity.Abilities.WarpedMagi
//{
//    [CreateAssetMenu(fileName = "WarpedMagi_MagicalShield.asset", menuName = "Ability/WarpedMagi/MagicalShield")]
//    public class MagicalShield : Ability
//    {
//        [SerializeField] private float _manaCost = 100f;
//        [SerializeField] private float _damage = 100f;
//        [SerializeField] private float _damageBlock = 50f;
//        [SerializeField] private float _damageRadius = 5f;
//
//		private ITeamable _teamable;
//		private IMagicable _magicable;
//
//	    public override void Terminate()
//	    {
//		    //Nothing to terminate
//	    }
//
//	    protected override void Initialize()
//        {
//			_teamable = Self.GetComponent<ITeamable>();
//			_magicable = Self.GetComponent<IMagicable>();
//        }
//
//        protected override void Cast()
//        {
//            _magicable.ModifyMana(-_manaCost, Self);
//            ApplyMagicalShield(Self);
//        }
//		public void ApplyMagicalShield(GameObject target)
//		{
//			var cols = Physics.OverlapSphere(target.transform.position, _damageRadius, (int)LayerMaskHelper.Entity); 
//			var gos = Triggers.Trigger.GetGameObjectFromColliders(cols);
//			var targetCount = 0;
//			
//			foreach(var go in gos)
//			{
//				if(go == target)
//					continue;
//				
////				if(_teamable == null || teamable == null || teamable.Team == _teamable.Team)
////					continue;
//				
//				ITeamable teamable = target.GetComponent<ITeamable>();
//				IHealthable healthable = target.GetComponent<IHealthable>();		
//
//				if(healthable == null)
//					continue;
//				
//				targetCount++;
//				
//				var damage = new Damage(_damage, DamageType.Magical, Self);
//				healthable.TakeDamage(damage);
//			}
//			
//			//TO DO apply damage block
//			//Dont have any way to damage block outside of buffs
//			
//		}
//    }
//}

