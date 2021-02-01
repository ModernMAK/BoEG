﻿using MobaGame.Framework.Core;
using MobaGame.Framework.Core.Modules;

namespace MobaGame.Assets.Scripts.Framework.Core.Modules
{
	public abstract class ActorItem : IItem, IListener<Actor>
	{
		public abstract void Unregister(Actor source);
		public abstract void Register(Actor source);

	}
	public abstract class OnAttackItem : ActorItem
	{

		public override void Register(Actor source)
		{
			if (source.TryGetModule<IAttackerable>(out var attackerable))
				attackerable.Attacking += OnAttack;
		}

		public override void Unregister(Actor source)
		{
			if (source.TryGetModule<IAttackerable>(out var attackerable))
				attackerable.Attacking -= OnAttack;
		}
		protected abstract void OnAttack(object sender, AttackerableEventArgs e);
	}
}