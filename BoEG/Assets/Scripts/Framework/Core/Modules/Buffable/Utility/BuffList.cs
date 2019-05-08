using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;

namespace Framework.Core.Modules
{
    public sealed class BuffList<T> : IBuffList<T>
    {
        private bool Convert(object value, out T result)
        {
            if (value is T casted)
            {
                result = casted;
                return true;
            }

            result = default(T);
            return false;
        }

        private void BuffRemoved(object sender, BuffEventArgs e)
        {
            T buff;
            if (Convert(e.Buff, out buff))
                Remove(buff);
        }

        private void BuffAdded(object sender, BuffEventArgs e)
        {
            T buff;
            if (Convert(e.Buff, out buff))
                Add(buff);
        }

        public void Subscribe(IBuffable buffable)
        {
            buffable.Added += BuffAdded;
            buffable.Removed += BuffRemoved;
        }

        public void Unsubscribe(IBuffable buffable)
        {
            buffable.Added -= BuffAdded;
            buffable.Removed -= BuffRemoved;
        }

        public event EventHandler<T> Added;
        public event EventHandler<T> Removed;

        public BuffList()
        {
            _backingList = new List<T>();
        }

        public BuffList(IEnumerable<T> capacity)
        {
            _backingList = new List<T>(capacity);
        }

        public BuffList(int capacity)
        {
            _backingList = new List<T>(capacity);
        }
        
        public BuffList(IBuffable buffable) : this()
        {
            Subscribe(buffable);
        }

        public BuffList(IEnumerable<T> capacity, IBuffable buffable) : this(capacity)
        {
            Subscribe(buffable);
        }

        public BuffList(int capacity, IBuffable buffable) : this(capacity)
        {
            Subscribe(buffable);
        }

        private readonly IList<T> _backingList;

        public IEnumerator<T> GetEnumerator()
        {
            return _backingList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _backingList).GetEnumerator();
        }

        public void Add(T item)
        {
            _backingList.Add(item);
            OnAdded(item);
        }

        public void Clear()
        {
            foreach (var item in _backingList)
            {
                OnRemoved(item);
            }

            _backingList.Clear();
        }

        public bool Contains(T item)
        {
            return _backingList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _backingList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            var result = _backingList.Remove(item);
            if (result)
                OnRemoved(item);
            return result;
        }

        public int Count => _backingList.Count;

        public bool IsReadOnly => _backingList.IsReadOnly;

        public int IndexOf(T item)
        {
            return _backingList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _backingList.Insert(index, item);
            OnAdded(item);
        }

        public void RemoveAt(int index)
        {
            var item = _backingList[index];
            _backingList.RemoveAt(index);
            OnRemoved(item);
        }

        public T this[int index]
        {
            get => _backingList[index];
            set
            {
                OnRemoved(_backingList[index]);
                _backingList[index] = value;
                OnAdded(value);
            }
        }

        private void OnAdded(T e)
        {
            Added?.Invoke(this, e);
        }

        private void OnRemoved(T e)
        {
            Removed?.Invoke(this, e);
        }
    }
}