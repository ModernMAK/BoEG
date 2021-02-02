using UnityEngine;
using MobaGame.Framework.Types;
namespace MobaGame.Framework.Core.Modules
{
	[CreateAssetMenu(menuName = "Item/CritItem")]
	public class CritItemObject : ItemObject, IListener<Actor>
	{
        [SerializeField]        private float _gradualChance;
        [SerializeField]        private float _critMultiplier;

		private CritItem _critItem;
		public CritItem CritItem =>_critItem ??= new CritItem(new CritEffect(new GradualProc(_gradualChance),_critMultiplier));
		public void Register(Actor source) => CritItem.Register(source);

		public void Unregister(Actor source) => CritItem.Unregister(source);
    }
}