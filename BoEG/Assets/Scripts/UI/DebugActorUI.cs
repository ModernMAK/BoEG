using MobaGame.Framework.Core;

namespace MobaGame.UI
{
    public abstract class DebugActorUI : DebugUI<Actor>
    {
    }
    public abstract class DebugModuleUI<TModule> : DebugActorUI
	{
		protected Actor Actor { get; private set; }
		public override void SetTarget(Actor target)
		{
			Actor = target;
			SetTarget(Actor != null ? Actor.GetModule<TModule>() : default);
		}
		public abstract void SetTarget(TModule target);
	}
}