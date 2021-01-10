using MobaGame.Framework.Core.Modules;
using UnityEngine;

namespace MobaGame.Entity.UnitArchtypes
{
    public interface IUnitData
    {
        AggroableData AggroableData { get; }
        HealthableData HealthableData { get; }
        MagicableData MagicableData { get; }
        ArmorableData ArmorableData { get; }
        AttackerableData AttackerableData { get; }
        MovableData MovableData { get; }
        Sprite Icon { get; }
    }
}