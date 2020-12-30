using System;

namespace Framework.Core.Modules
{
    public interface ILevelable
    {
        event EventHandler<int> LevelChanged;
    }
}