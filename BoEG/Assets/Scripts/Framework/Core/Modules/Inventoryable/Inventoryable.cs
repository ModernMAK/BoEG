using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;

namespace MobaGame.Assets.Scripts.Framework.Core.Modules
{
	public class Inventoryable<T> : ActorModule, IInventoryable<T> where T : IItem
	{
		public Inventoryable(Actor actor, IInventory<T> inventory) : base(actor)
		{
			Inventory = inventory;
			Inventory.Added += Inventory_Added;
			Inventory.Removed += Inventory_Removed;
		}

		private void Inventory_Removed(object sender, T e)
		{
			if (e is IListener<Actor> listener)
				listener.Unregister(Actor);
		}

		private void Inventory_Added(object sender, T e)
		{
			if (e is IListener<Actor> listener)
				listener.Register(Actor);
		}

		public IInventory<T> Inventory { get; }


	}

}