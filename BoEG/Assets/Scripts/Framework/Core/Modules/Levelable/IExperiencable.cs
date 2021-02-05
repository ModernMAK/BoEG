using MobaGame.Framework.Core.Modules;
using System;

namespace MobaGame.Framework.Core
{
	public interface IExperiencable
	{

		event EventHandler<ChangedEventArgs<int>> ExperienceChanged;
		int Experience { get; set; }
		int ExperienceNeeded { get; }
		int ExperienceCapacity { get; }
	}
}