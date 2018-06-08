using System;
using UnityEngine;

namespace Components.Movable
{
    [Obsolete]
    public interface IMovable
    {
        IMovableInstance Movable { get; }
    }
}