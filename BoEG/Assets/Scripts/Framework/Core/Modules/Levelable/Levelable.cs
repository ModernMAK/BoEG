using MobaGame.Framework.Core.Modules;
using System;

namespace MobaGame.Framework.Core
{
	public class Levelable : ActorModule, ILevelable
	{
		public Levelable(Actor actor) : base(actor)
		{
		}

		private int _level;
		public int Level 
		{
			get => _level;
			set=>ChangedEventUtil.SetValueClamped(ref _level, value, 0, MaxLevel, OnLevelChanged);

		}

		protected void OnLevelChanged(ChangedEventArgs<int> e)
		{
			LevelChanged?.Invoke(this, e);
		}

		public int MaxLevel { 
			get;
			private set;
		}

		public event EventHandler<ChangedEventArgs<int>> LevelChanged;

	}
}