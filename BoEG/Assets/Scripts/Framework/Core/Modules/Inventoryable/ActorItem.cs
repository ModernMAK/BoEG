using MobaGame.Framework.Core;

namespace MobaGame.Framework.Core.Modules
{
	public abstract class ActorItem : IItem, IListener<Actor>
	{
		public abstract void Unregister(Actor source);
		public abstract void Register(Actor source);

	}
}