namespace Components.Attackerable
{
    public interface IAttackerable
    {
        float Damage { get; }
        float AttackRange { get; }
        float AttackSpeed { get; }
    }
}