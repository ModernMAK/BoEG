using System;
using System.Collections.Generic;

namespace MobaGame.Framework.Core.Modules
{
	public interface IInventory<T> : IList<T>, IReadOnlyList<T> where T : IItem
	{
		event EventHandler<T> Added;
		event EventHandler<T> Removed;
	}

}