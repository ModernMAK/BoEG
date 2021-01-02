namespace Framework.Ability
{
    public interface IObjectTargetAbility<TObject> : IAbility
    {
        void CastObjectTarget(TObject target);
    }
}