using System;

namespace MobaGame.Framework.Core.Modules
{
	public class ChangableEventArgs<T> : EventArgs
	{
		public ChangableEventArgs(T before)
		{
			Before = before;
			After = before;
		}
		public ChangableEventArgs(T before, T after)
		{
			Before = before;
			After = after;
		}
		public T Before { get; }
		public T After { get; set; }
	}
}