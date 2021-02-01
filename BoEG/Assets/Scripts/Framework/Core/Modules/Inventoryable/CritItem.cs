using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Types;

namespace MobaGame.Assets.Scripts.Framework.Core.Modules
{
	public class CritItem : ActorItem
	{
		public CritItem(float procChance, float critMultiplier)
		{
			_chance = new GradualProc(procChance);
			_critMultiplier = critMultiplier;
		}
		private readonly GradualProc _chance;
		private readonly float _critMultiplier;
		private void CritProc(object sender, AttackCritEventArgs e)
		{
			if (e.CriticalMultiplier > _critMultiplier)
				return;

			if (_chance.Proc())
			{
				e.CriticalMultiplier = _critMultiplier;
			}
		}
		public override void Register(Actor source)
		{
			if (source.TryGetModule<IAttackerable>(out var attackerable))
				attackerable.CritModifiers += CritProc;
		}

		public override void Unregister(Actor source)
		{
			if (source.TryGetModule<IAttackerable>(out var attackerable))
				attackerable.CritModifiers -= CritProc;
		}
	}
}