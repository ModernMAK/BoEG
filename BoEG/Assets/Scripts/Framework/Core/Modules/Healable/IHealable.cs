using MobaGame.Framework.Types;
using System;

namespace MobaGame.Framework.Core.Modules
{

	public interface IHealable
	{
		void Heal(SourcedHeal heal);
		event EventHandler<SourcedHeal> Healing;
		event EventHandler<SourcedHeal> Healed;
		event EventHandler<ChangableEventArgs<SourcedHeal>> HealingModifiers;
	}
}