namespace Framework.Core.Modules
{
    public interface IListener<in T>
    {
        void Register(T source);
        void Unregister(T source);
    }
}