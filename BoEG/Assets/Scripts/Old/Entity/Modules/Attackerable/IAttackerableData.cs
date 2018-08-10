using UnityEngine;

namespace Old.Entity.Modules.Attackerable
{
    public interface IAttackerableData
    {
        float BaseDamage { get; }
        float BaseAttackRange { get; }
        float BaseAttackSpeed { get; }
        float GainDamage { get; }
        float GainAttackSpeed { get; }
        GameObject Projectile { get; }
    }
}