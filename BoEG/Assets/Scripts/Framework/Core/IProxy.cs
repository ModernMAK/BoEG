namespace Framework.Core
{
    public interface IProxy<out T>
    {
        T Value { get; }
    }
}