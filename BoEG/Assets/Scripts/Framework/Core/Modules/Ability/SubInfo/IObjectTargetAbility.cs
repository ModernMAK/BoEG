namespace MobaGame.Framework.Core.Modules.Ability
{
    public interface IObjectTargetAbility<in TObject> : IAbility
    {
        void CastObjectTarget(TObject target);
    }
}