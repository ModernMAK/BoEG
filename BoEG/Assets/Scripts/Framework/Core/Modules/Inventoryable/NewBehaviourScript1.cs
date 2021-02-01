using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Types;

namespace MobaGame.Assets.Scripts.Framework.Core.Modules
{
	public interface IInventory<T>
	{
		int Size { get; }
		T this[int index] { get; set; }

		

	}
	public interface IItem 
	{
		
	}
	public abstract class OnAttackItem : IItem, IListener<Actor>
	{

		public void Register(Actor source)
		{
			if (source.TryGetModule<IAttackerable>(out var attackerable))
				attackerable.Attacking += OnAttack;
		}

		public void Unregister(Actor source)
		{
			if (source.TryGetModule<IAttackerable>(out var attackerable))
				attackerable.Attacking -= OnAttack;
		}
		protected abstract void OnAttack(object sender, AttackerableEventArgs e);
	}
	public class CritItem : OnAttackItem
	{
		public CritItem(float procChance, float critMultiplier)
		{
			_chance = new GradualProc(procChance);
			_critMultiplier = critMultiplier;
		}
		private readonly GradualProc _chance;
		private readonly float _critMultiplier;
		protected override void OnAttack(object sender, AttackerableEventArgs e)
		{
			if (_chance.Proc())
			{
				e.Damage = e.Damage.SetValue(e.Damage.Value * _critMultiplier, true);
			}
		}
	}
}