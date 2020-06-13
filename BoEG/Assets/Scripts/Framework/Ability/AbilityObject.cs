using System.Net.Http.Headers;
using Framework.Core;
using UnityEngine;

namespace Entity.Abilities.FlameWitch
{
    public class AbilityObject : ScriptableObject, IAbility, IAbilityView
    {
        [SerializeField] private Sprite _icon;
        public Actor Self { get; private set; }

        public virtual void Initialize(Actor actor)
        {
            Self = actor;
            AbilityHelper.Initialize();//HACK
        }


        /// <summary>
        /// Used to ask the Ability to setup Ability Preview
        /// </summary>
        public virtual void SetupCast()
        {
        }

        /// <summary>
        /// Used to confirm the Ability, if the ability cannot cast, the ability 
        /// </summary>
        public virtual void ConfirmCast()
        {
        }

        /// <summary>
        /// Used to reset the Ability Preview
        /// </summary>
        public virtual void CancelCast()
        {
        }

        public virtual IAbilityView GetAbilityView() => this;
        public virtual Sprite GetIcon() => _icon;

        public virtual float GetCooldownProgress() => 1f;

        public virtual float GetManaCost() => 0f;
    }
}