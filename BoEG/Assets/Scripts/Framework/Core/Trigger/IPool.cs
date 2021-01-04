namespace MobaGame.Framework.Core.Trigger
{
    public interface IPool<T>
    {
        T Allocate();
        void Allocate(ref T poolable);
        void Release(ref T poolable);
    }
}