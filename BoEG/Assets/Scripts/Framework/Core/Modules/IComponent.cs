namespace Framework.Core.Modules
{
    //TODO rename to IInitializable
    public interface IComponent<T>
    {
        void Initialize(T module);
    }
}