using UnityEngine;
using UnityEngine.Analytics;

namespace Framework.Core.Modules
{
    public interface IAttackerable
    {
        float AttackRange { get; }
        float AttackDamage { get; }
        float AttackSpeed { get; }
        bool IsRanged { get; }
        void Attack(GameObject target);
        bool InRange(GameObject target);
        bool CanAttack { get; }
    }
}