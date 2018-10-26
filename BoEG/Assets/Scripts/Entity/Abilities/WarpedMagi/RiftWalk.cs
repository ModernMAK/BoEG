using Modules.Abilityable;
using Modules.Magicable;
using Modules.Movable;
using UnityEngine;

namespace Entity.Abilities.WarpedMagi
{
    [CreateAssetMenu(fileName = "WarpedMagi_RiftWalk.asset", menuName = "Ability/WarpedMagi/RiftWalk")]
    public class RiftWalk : BetterAbility
    {
        [SerializeField] private float _manaCost = 100f;
        [SerializeField] private float _channeltime = 1f;
		[SerializeField] private float _cooldown = 30f;

	    public override float ManaCost
	    {
		    get { return _manaCost; }
	    }

	    public override float MaxChannelDuration
	    {
		    get { return _channeltime; }
	    }

	    public override float Cooldown
	    {
		    get { return _cooldown; }
	    }


		private IMovable _movable;
		private IMagicable _magicable;

	    public override void Terminate()
	    {
		    //Nothing to Terminate
	    }

        public override void Initialize(GameObject go)
        {
	        base.Initialize(go);
			_movable = Self.GetComponent<IMovable>();
			_magicable = Self.GetComponent<IMagicable>();
        }

	    protected override void Prepare()
	    {
		    
	    }

	    protected override void Cast()
	    {
		    RaycastHit hit;
		    if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) 
			    return;
		    if (!InCastRange(hit.point))
			    return;
		    
		    SpendMana();
		    GroundCast(hit.point);
	    }

	    public override void GroundCast(Vector3 point)
	    {
		    //TODO perform Channel, and introduce Cooldown
		    _movable.Teleport(point);
	    }

    }
}