using System;
using System.Collections.Generic;

namespace MobaGame.Framework.Core.Modules
{
	public class UnlimitedInventory<T> : AbstractInventory<T> where T : IItem
	{
		public UnlimitedInventory() : base(new List<T>())
		{

		}

		public override void Add(T item)
		{
			if(Contains(item))
				throw new ArgumentException("This item already exists in the inventory!");
			InternalList.Add(item);
			OnAdded(item);
		}

		public override void Clear()
		{
			for (var i = InternalList.Count - 1; i >= 0; i--)
				RemoveAt(i);
		}

		public override void Insert(int index, T item)
		{
			if (index < 0 || index > Count)
				throw new Exception("Slot does not exist!");
			if (InternalList[index] != null)
			{
				InternalList.RemoveAt(index);
				OnRemoved(item);
			}
			InternalList[index] = item;
			OnAdded(item);
		}

		public override bool Remove(T item)
		{
			if (InternalList.Remove(item))
			{				
				OnRemoved(item);
				return true;
			}
			return false;
		}

		public override void RemoveAt(int index)
		{
			if (index < 0 || index > Count)
				throw new Exception("Slot does not exist!");
			if (InternalList[index] != null)
			{
				var item = InternalList[index];
				InternalList.RemoveAt(index);
				OnRemoved(item);
			}
		}
	}

}