namespace MobaGame.Framework.Core.Modules
{
    //TODO rename to IInitializable
    public interface IInitializable<T>
    {
        void Initialize(T module);
    }
}