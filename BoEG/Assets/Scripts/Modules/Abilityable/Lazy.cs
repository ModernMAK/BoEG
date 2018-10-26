//using System;
//
//namespace Util
//{
//    
//
//    public class Lazy<T>
//    {
//        public Lazy(Func<T> constructor)
//        {
//            _constructor = constructor;
//            _init = false;
//        }
//        private T _instance;
//        private bool _init;
//        private readonly Func<T> _constructor;
//
//        public T Instance
//        {
//            get
//            {
//                if (_init) return _instance;
//                _instance = _constructor();
//                _init = true;
//                return _instance;
//            }
//        }
//
//        public static implicit operator T(Lazy<T> lazy)
//        {
//            return lazy.Instance;
//        }
//    }
//}
////How I think Lazy is implimented 