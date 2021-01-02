using Framework.Core;
using Framework.Core.Modules;

namespace Framework.Ability
{
    public interface IAbility : IInitializable<Actor>
    {
        IAbilityView GetAbilityView();

        /// <summary>
        ///     Used to ask the Ability to setup Ability Preview
        /// </summary>
        void SetupCast();

        /// <summary>
        ///     Used to confirm the Ability, if the ability cannot cast, the ability
        /// </summary>
        void ConfirmCast();

        /// <summary>
        ///     Used to reset the Ability Preview
        /// </summary>
        void CancelCast();
    }
}