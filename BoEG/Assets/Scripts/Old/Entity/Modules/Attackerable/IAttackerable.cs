using UnityEngine;

namespace Old.Entity.Modules.Attackerable
{
    public interface IAttackerable
    {
        float Damage { get; }
        float AttackRange { get; }
        float AttackSpeed { get; }
        GameObject Projectile { get; }
        void Attack(GameObject go);
        event AttackHandler AttackLaunched;
        event AttackHandler AttackLanded;
        event AttackHandler AttackKilled;
    }
}