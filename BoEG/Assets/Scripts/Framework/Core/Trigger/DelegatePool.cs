using System;

namespace Framework.Core.Trigger
{
    public class DelegatePool<T> : Pool<T>
    {
        private readonly Action<T> _allocate;
        private readonly Func<T> _create;
        private readonly Action<T> _destroy;
        private readonly Action<T> _release;

        public DelegatePool(Func<T> create, Action<T> allocate, Action<T> release, Action<T> destroy, int limit) :
            base(limit)
        {
            _create = create;
            _allocate = allocate;
            _release = release;
            _destroy = destroy;
        }

        protected override T Create()
        {
            return _create();
        }

        protected override void Destroy(T poolable)
        {
            _destroy(poolable);
        }

        protected override void Release(T poolable)
        {
            _release(poolable);
        }

        protected override void Allocate(T poolable)
        {
            _allocate(poolable);
        }
    }
}