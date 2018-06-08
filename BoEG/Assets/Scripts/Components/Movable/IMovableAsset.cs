using System;

namespace Components.Movable
{
    [Obsolete]
    public interface IMovableAsset
    {
        IMovableData MovableData { get; }
    }
}