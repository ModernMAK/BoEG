namespace Old.Entity.Modules.Abilityable
{
    public interface IAbilityable
    {
        void Cast(int index);
        T GetAbility<T>() where T : IAbility;
    }
}