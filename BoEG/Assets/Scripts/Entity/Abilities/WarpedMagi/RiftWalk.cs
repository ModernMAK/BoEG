using Core;
using Modules.Abilityable;
using Modules.Magicable;
using Modules.MiscEvents;
using Triggers;
using Modules.Healthable;
using UnityEngine;
using Util;

namespace Entity.Abilities.WarpedMagi
{
    [CreateAssetMenu(fileName = "WarpedMagi_RiftWalk.asset", menuName = "Ability/WarpedMagi/RiftWalk")]
    public class RiftWalk : Ability
    {
        [SerializeField] private float _manaCost = 100f;
        [SerializeField] private float _channeltime = 1f;
		[SerializeField] private float _cooldown = 30f;
		

		
        private GameObject _self;
		private IMovable _movable;
		private IMagicable _magicable;


        public override void Initialize(GameObject go)
        {
            _self = go;
			_movable = go.GetComponet<IMovable>();
			_magicable = go.GetComponet<IMagicable>();
        }

        public override void Trigger()
        {
            RaycastHit hit;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) 
				return;
						
            _magicable.ModifyMana(-_manaCost, _self);
            ApplyMagicalShield(hit.point);
        }
		public void ApplyRiftWalk(Vector3 position)
		{
			//TODO perform Channel, and introduce Cooldown
			_movable.Teleport(position);
		}
    }
}