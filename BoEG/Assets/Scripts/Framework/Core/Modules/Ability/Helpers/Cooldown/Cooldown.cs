using MobaGame.Framework.Types;
using System;

namespace MobaGame.Framework.Core.Modules.Ability.Helpers
{
	public class Cooldown : CooldownBase
	{
		public Cooldown(float duration) : this(new DurationTimer(duration, true)) { }
		public Cooldown(DurationTimer internalTimer)
		{
			Timer = internalTimer;
		}

	}
}