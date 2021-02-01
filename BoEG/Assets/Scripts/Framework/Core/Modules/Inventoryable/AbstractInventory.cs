using System;
using System.Collections;
using System.Collections.Generic;

namespace MobaGame.Assets.Scripts.Framework.Core.Modules
{
	public abstract class AbstractInventory<T> : IInventory<T> where T : IItem
	{
		protected IList<T> InternalList { get; }
		protected AbstractInventory(IList<T> list)
		{
			InternalList = list; 
		}
		public T this[int index] {
			get => InternalList[index];
			set => Insert(index, value); 
		}

		public int Count => InternalList.Count;

		public bool IsReadOnly => false;

		public event EventHandler<T> Added;
		public event EventHandler<T> Removed;

		public abstract void Add(T item);

		public abstract void Clear();

		public bool Contains(T item) => InternalList.Contains(item);

		public void CopyTo(T[] array, int arrayIndex) => InternalList.CopyTo(array, arrayIndex);

		public virtual IEnumerator<T> GetEnumerator() => InternalList.GetEnumerator();

		public int IndexOf(T item) => InternalList.IndexOf(item);

		public abstract void Insert(int index, T item);

		protected void OnAdded(T e) => Added?.Invoke(this,e);
		protected void OnRemoved(T e) => Removed?.Invoke(this,e);


		public abstract bool Remove(T item);

		public abstract void RemoveAt(int index);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

}