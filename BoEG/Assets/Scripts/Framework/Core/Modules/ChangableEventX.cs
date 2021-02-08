using System;

namespace MobaGame.Framework.Core.Modules
{
	public static class ChangableEventX
	{
		public static T CalculateChange<T>(this EventHandler<ChangableEventArgs<T>> handler, object sender, T before)
		{
			var e = new ChangableEventArgs<T>(before);
			handler?.Invoke(sender, e);
			return e.After;
		}
	}
}