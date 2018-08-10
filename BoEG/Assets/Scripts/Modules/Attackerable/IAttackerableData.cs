using Core;
using UnityEngine;

namespace Modules.Attackerable
{
    public interface IAttackerableData
    {
        FloatScalar AttackDamage { get; }
        FloatScalar AttackSpeed { get; }
        float AttackRange { get; }
        GameObject Projectile { get; }
    }
}