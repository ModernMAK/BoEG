namespace Framework.Core.Modules
{
    public interface IAttackerableData
    {
        float AttackRange { get; }
        float AttackDamage { get; }
        float AttackSpeed { get; }
        bool IsRanged { get; }
    }
}