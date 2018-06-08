using Core;

namespace Components.Attackerable
{
    public interface IAttackerableInstance
    {
        float Damage { get; }
        float AttackRange { get; }
        float AttackSpeed { get; }
    }
}