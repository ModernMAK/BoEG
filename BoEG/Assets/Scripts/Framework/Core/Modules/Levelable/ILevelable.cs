using MobaGame.Framework.Core.Modules;
using System;

namespace MobaGame.Framework.Core
{
	public interface ILevelable
    {
        event EventHandler<ChangedEventArgs<int>> LevelChanged;

        int Level { get; set; }
        int MaxLevel { get; }
    }
}