namespace MobaGame.Framework.Core.Modules
{
	public class CritItem : ActorItem
	{
		public CritItem(CritEffect crit)
		{
			_crit = crit;
		}
		private readonly CritEffect _crit;

		public override void Register(Actor source)
		{
			if (source.TryGetModule<IAttackerable>(out var attackerable))
				_crit.Register(attackerable);
		}

		public override void Unregister(Actor source)
		{
			if (source.TryGetModule<IAttackerable>(out var attackerable))
				_crit.Unregister(attackerable);
		}
	}
}