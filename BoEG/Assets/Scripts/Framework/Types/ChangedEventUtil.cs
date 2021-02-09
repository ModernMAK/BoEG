using MobaGame.Framework.Core.Modules;
using System;

namespace MobaGame.Framework.Core
{
	public static class ChangedEventUtil
	{
		public static void SetValueClamped(ref int original, int value, int min, int max, Action<ChangedEventArgs<int>> invoke)
		{

			var old = original;
			var @new = Math.Max(min, Math.Min(value, max));
			var changed = old != @new;
			original = @new;
			if (changed)
				invoke(new ChangedEventArgs<int>(old, @new));
		}
	}
}