using System;

namespace MobaGame.Framework.Core
{
    public interface ILevelable
    {
        event EventHandler<int> LevelChanged;
    }
}