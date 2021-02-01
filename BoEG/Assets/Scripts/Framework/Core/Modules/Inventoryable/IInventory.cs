namespace MobaGame.Assets.Scripts.Framework.Core.Modules
{
	public interface IInventory<T>
	{
		int Size { get; }
		T this[int index] { get; set; }

		

	}
}