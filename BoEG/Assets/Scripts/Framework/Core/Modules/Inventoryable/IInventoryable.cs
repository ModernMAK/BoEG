namespace MobaGame.Framework.Core.Modules
{
	public interface IInventoryable<T> where T : IItem
	{
		IInventory<T> Inventory { get; }
	}

}