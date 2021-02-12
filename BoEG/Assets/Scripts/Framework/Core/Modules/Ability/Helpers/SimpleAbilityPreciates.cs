using System;

namespace MobaGame.Framework.Core.Modules.Ability.Helpers
{
	/// <summary>
	/// Generic container for a slef check
	/// </summary>
	public static class SimpleAbilityPreciates
	{
        private static bool InternalIsSelf(Actor self, Actor other) => self == other;
		public static Func<Actor, bool> IsSelf(Actor self) => PartialFunctions.Partial<Actor, Actor, bool>(InternalIsSelf, self);
		public static Func<bool> IsDone(Cooldown cd)
		{
			bool wrapper() => cd.IsDone;
			return wrapper;
		}
	}
}