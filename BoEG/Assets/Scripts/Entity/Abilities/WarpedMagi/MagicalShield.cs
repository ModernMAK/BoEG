using Core;
using Modules.Abilityable;
using Modules.Magicable;
using Modules.Healthable;
using Modules.Teamable;
using Triggers;
using UnityEngine;
using Util;

namespace Entity.Abilities.WarpedMagi
{
    [CreateAssetMenu(fileName = "WarpedMagi_MagicalShield.asset", menuName = "Ability/WarpedMagi/MagicalShield")]
    public class MagicalShield : Ability
    {
        [SerializeField] private float _manaCost = 100f;
        [SerializeField] private float _damage = 100f;
        [SerializeField] private float _damageBlock = 50f;
        [SerializeField] private float _damageRadius = 5f;

		
        private GameObject _self;
		private ITeamable _teamable;
		private IMagicable _magicable;

	    public override void Terminate()
	    {
		    //TODO
		    throw new System.NotImplementedException();
	    }

        public override void Initialize(GameObject go)
        {
            _self = go;
			_teamable = go.GetComponent<ITeamable>();
			_magicable = go.GetComponent<IMagicable>();
        }

        public override void Trigger()
        {
            _magicable.ModifyMana(-_manaCost, _self);
            ApplyMagicalShield(_self);
        }
		public void ApplyMagicalShield(GameObject target)
		{
			var cols = Physics.OverlapSphere(target.transform.position, _damageRadius, (int)LayerMaskHelper.Entity); 
			var gos = Triggers.Trigger.GetGameObjectFromColliders(cols);
			var targetCount = 0;
			
			foreach(var go in gos)
			{
				if(go == target)
					continue;
				
//				if(_teamable == null || teamable == null || teamable.Team == _teamable.Team)
//					continue;
				
				ITeamable teamable = target.GetComponent<ITeamable>();
				IHealthable healthable = target.GetComponent<IHealthable>();		

				if(healthable == null)
					continue;
				
				targetCount++;
				
				var damage = new Damage(_damage, DamageType.Magical, _self);
				healthable.TakeDamage(damage);
			}
			
			//TODO apply damage block
			//Dont have any way to damage block outside of buffs
			
		}
    }
}