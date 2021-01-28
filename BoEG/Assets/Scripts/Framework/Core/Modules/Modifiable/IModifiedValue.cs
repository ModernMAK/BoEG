namespace MobaGame.Framework.Core.Modules
{
	public interface IModifiedValue<T>
	{
        public T Base { get; }
        public T Bonus { get; }
        public T Total { get; }
    }

}