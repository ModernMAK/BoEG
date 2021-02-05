using MobaGame.Framework.Core.Modules;
using System;
using System.Collections.Generic;

namespace MobaGame.Framework.Core
{
	public class ExperienceLevelable : ActorModule, IExperiencable, ILevelable, IInitializable<int[]>
	{
		public ExperienceLevelable(Actor actor) : base(actor)
		{
			_experienceToLevel = new List<int>();
			_level = 1;

		}

		private int _level;
		private int _experience;
		private readonly List<int> _experienceToLevel;
		public int Level
		{
			get => _level;
			set => ChangedEventUtil.SetValueClamped(ref _level, value, 0, MaxLevel, OnLevelChanged);
		}

		private void OnLevelChanged(ChangedEventArgs<int> e) => LevelChanged?.Invoke(this, e);

		int ILevelable.Level
		{
			get => Level;
			set => throw new NotSupportedException();
		}
		public int Experience
		{
			get => _experience;
			set
			{
				ChangedEventUtil.SetValueClamped(ref _experience, value, 0, ExperienceCapacity, OnExperienceChanged);
				RecalculateLevel();
			}
		}

		private void RecalculateLevel()
		{
			var oldLevel = _level;
			if (Level != MaxLevel)
			{
				if (Experience == ExperienceCapacity)
					_level = MaxLevel;
				else
					while (Experience >= ExperienceNeeded)
						_level += 1;
			}
			var newLevel = _level;
			if (oldLevel != newLevel)
				OnLevelChanged(new ChangedEventArgs<int>(oldLevel, newLevel));

		}

		private void OnExperienceChanged(ChangedEventArgs<int> e) => ExperienceChanged?.Invoke(this,e);

		public void Initialize(int[] data)
		{
			_experienceToLevel.Clear();
			_experienceToLevel.Add(0);
			var total = 0;
			for (var i = 0; i < data.Length; i++)
			{
				total += data[i];
				_experienceToLevel.Add(total);
			}
		}

		public int ExperienceNeeded => _experienceToLevel[_level];

		public int ExperienceCapacity => _experienceToLevel[_experienceToLevel.Count-1];

		public int MaxLevel => _experienceToLevel.Count-1;//Don't include 0

		public event EventHandler<ChangedEventArgs<int>> ExperienceChanged;
		public event EventHandler<ChangedEventArgs<int>> LevelChanged;
	}
}