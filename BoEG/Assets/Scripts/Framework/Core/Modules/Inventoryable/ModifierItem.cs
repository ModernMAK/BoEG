using MobaGame.Framework.Core;

namespace MobaGame.Assets.Scripts.Framework.Core.Modules
{
	public abstract class ModifierItem : ActorItem, IListener<IModifiable>
	{
		public override void Register(Actor source)
		{
			if (source.TryGetModule<IModifiable>(out var m))
				Register(m);
		}
		public override void Unregister(Actor source)
		{
			if (!source.TryGetModule<IModifiable>(out var m))
				Register(m);
		}
		public abstract void Register(IModifiable source);

		public abstract void Unregister(IModifiable source);
	}
}