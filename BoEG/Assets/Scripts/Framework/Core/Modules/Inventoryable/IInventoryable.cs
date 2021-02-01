namespace MobaGame.Assets.Scripts.Framework.Core.Modules
{
	public interface IInventoryable<T> where T : IItem
	{
		IInventory<T> Inventory { get; }
	}

}