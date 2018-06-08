namespace Components.Attackerable
{
    public interface IAttackerableData
    {
        float BaseDamage { get; }
        float BaseAttackRange { get; }
        float BaseAttackSpeed { get; }
        float GainDamage { get; }
        float GainAttackRange { get; }
        float GainAttackSpeed { get; }
    }
}