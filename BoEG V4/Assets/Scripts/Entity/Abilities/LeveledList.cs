using System.Collections.Generic;
using UnityEngine;

namespace Entity.Abilities
{
    public static class LeveledListHelper
    {
        public static T GetLeveled<T>(this IList<T> arr, int level = -1, T defValue = default(T))
        {
            level = Mathf.Max(level - 1, -1, arr.Count);
            return level >= 0 ? arr[level] : defValue;
        }
    }
}
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//namespace Entity.Abilities
//{
//    [Serializable]
//    public class LeveledList<T> : IList<T>
//    {
//        [SerializeField] private T[] _arr;
//
//        public LeveledList(int size)
//        {
//            _arr = new T[size];
//        }
//
//        public LeveledList(T[] arr)
//        {
//            _arr = new T[arr.Length];
//            arr.CopyTo(_arr, 0);
//        }
//
//        public T Get(int index = -1, T defaultValue = default(T))
//        {
//            index = Mathf.Clamp(index, -1, Count - 1);
//            return index >= 0 ? this[index] : defaultValue;
//            
//        }
//
//        private IList<T> List
//        {
//            get { return _arr; }
//        }
//
//        public IEnumerator<T> GetEnumerator()
//        {
//            return List.GetEnumerator();
//        }
//
//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return List.GetEnumerator();
//        }
//
//        void ICollection<T>.Add(T item)
//        {
//            List.Add(item);
//        }
//
//        public void Clear()
//        {
//            List.Clear();
//        }
//
//        bool ICollection<T>.Contains(T item)
//        {
//            return List.Contains(item);
//        }
//
//        public void CopyTo(T[] array, int arrayIndex)
//        {
//            List.CopyTo(array, arrayIndex);
//        }
//
//        bool ICollection<T>.Remove(T item)
//        {
//            return List.Remove(item);
//        }
//
//        public int Count
//        {
//            get { return List.Count; }
//        }
//
//        bool ICollection<T>.IsReadOnly
//        {
//            get { return List.IsReadOnly; }
//        }
//
//        public int IndexOf(T item)
//        {
//            return List.IndexOf(item);
//        }
//
//        void IList<T>.Insert(int index, T item)
//        {
//            List.Insert(index, item);
//        }
//
//        void IList<T>.RemoveAt(int index)
//        {
//            List.RemoveAt(index);
//        }
//
//        public T this[int index]
//        {
//            get { return List[index]; }
//            set { List[index] = value; }
//        }
//    }
//}