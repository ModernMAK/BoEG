namespace Modules.Abilityable
{
    public interface IAbilitiable
    {
        void Cast(int index);
        T GetAbility<T>() where T : IAbility;
    }
}