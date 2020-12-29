using Entity.Abilities.FlameWitch;
using Framework.Core;
using UnityEngine;

namespace Framework.Ability
{
    public class AbilityObject : ScriptableObject, IAbility, IAbilityView
    {
#pragma warning disable 0649

        [SerializeField] private Sprite _icon;
        protected CommonAbilityInfo _commonAbilityInfo;
#pragma warning disable 0649

        protected CommonAbilityInfo Helper => _commonAbilityInfo;
        public Actor Self { get; private set; }
        protected bool IsSelf(GameObject gameObject) => gameObject == Self.gameObject;
        protected bool IsSelf(Actor actor) => actor == Self;

        public virtual void Initialize(Actor actor)
        {
            Self = actor;
            AbilityHelper.Initialize(); //HACK TODO make this not a hack
            _commonAbilityInfo = new CommonAbilityInfo(actor);
        }


        /// <summary>
        ///     Used to ask the Ability to setup Ability Preview
        /// </summary>
        public virtual void SetupCast()
        {
        }

        /// <summary>
        ///     Used to confirm the Ability, if the ability cannot cast, the ability
        /// </summary>
        public virtual void ConfirmCast()
        {
        }

        /// <summary>
        ///     Used to reset the Ability Preview
        /// </summary>
        public virtual void CancelCast()
        {
        }

        public virtual IAbilityView GetAbilityView()
        {
            return this;
        }

        public virtual Sprite GetIcon()
        {
            return _icon;
        }

        public virtual float GetCooldownProgress()
        {
            return 1f;
        }

        public virtual float GetManaCost()
        {
            return 0f;
        }
    }
}