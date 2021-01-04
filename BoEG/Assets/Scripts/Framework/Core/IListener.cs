namespace MobaGame.Framework.Core
{
    public interface IListener<in T>
    {
        void Register(T source);
        void Unregister(T source);
    }
}