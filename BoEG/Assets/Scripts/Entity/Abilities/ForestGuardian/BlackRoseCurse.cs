using MobaGame.Framework.Core.Modules;
using MobaGame.Framework.Core.Modules.Ability;
using System;

namespace MobaGame.Entity.Abilities.ForestGuardian
{
	public class BlackRoseCurse : AbilityObject, IToggleableAbilityView
	{
		public bool Active => throw new System.NotImplementedException();

		public event EventHandler<ChangedEventArgs<bool>> Toggled;
	}
}