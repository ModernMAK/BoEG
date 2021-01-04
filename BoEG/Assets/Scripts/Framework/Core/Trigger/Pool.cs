using System.Collections.Generic;

namespace MobaGame.Framework.Core.Trigger
{
    public abstract class Pool<T> : IPool<T>
    {
        private readonly int _limit;

        private readonly Queue<T> _pool;

        protected Pool(int limit = 0)
        {
            _limit = limit;
            _pool = new Queue<T>();
        }

        public T Allocate()
        {
            var next = GetNext();
            Allocate(next);
            return next;
        }

        public void Allocate(ref T poolable)
        {
            poolable = Allocate();
        }


        public void Release(ref T poolable)
        {
            if (_limit > 0 && _pool.Count == _limit)
            {
                Destroy(poolable);
            }
            else
            {
                _pool.Enqueue(poolable);
                Release(poolable);
            }

            poolable = default;
        }

        private T GetNext()
        {
            if (_pool.Count > 0)
                return _pool.Dequeue();
            return Create();
        }

        protected abstract T Create();
        protected abstract void Allocate(T poolable);
        protected abstract void Release(T poolable);
        protected abstract void Destroy(T poolable);
    }
}