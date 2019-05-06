namespace Framework.Core.Modules
{
    public interface IComponent<T>
    {
        void Initialize(T module);

    }
}