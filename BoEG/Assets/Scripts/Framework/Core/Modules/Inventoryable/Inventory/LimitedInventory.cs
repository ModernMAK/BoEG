using System;
using System.Collections.Generic;

namespace MobaGame.Framework.Core.Modules
{
	public class LimitedInventory<T>  : AbstractInventory<T> where T : IItem
	{
		public LimitedInventory(int slots) : base(new T[slots])
		{
		}

		public override void Add(T item)
		{
			const int InvalidSlot = -1;
			int emptySlot = InvalidSlot;
			for(var i = 0; i < InternalList.Count; i++)
			{
				if (ReferenceEquals(InternalList[i],item))
					throw new ArgumentException("This item already exists in the inventory!");
				if (emptySlot == InvalidSlot && InternalList == null)
					emptySlot = i;
			}
			if (emptySlot == InvalidSlot)
				throw new ArgumentException("No more slots in inventory!");
			InternalList[emptySlot] = item;
			OnAdded(item);
		}

		public override void Clear()
		{
			for (var i = InternalList.Count-1; i >=0; i--)
				RemoveAt(i);
		}

		public override void Insert(int index, T item)
		{
			if (index < 0 || index > Count)
				throw new Exception("Slot does not exist!");
			if(InternalList[index] != null)
			{
				InternalList.RemoveAt(index);
				OnRemoved(item);
			}
			InternalList[index] = item;
			OnAdded(item);
		}

		public override bool Remove(T item)
		{
			var index = InternalList.IndexOf(item);
			if (index != -1)
			{
				InternalList.RemoveAt(index);
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
		public override IEnumerator<T> GetEnumerator()
		{
			foreach (var item in InternalList)
				if (item != null)
					yield return item;
		}
	}

}