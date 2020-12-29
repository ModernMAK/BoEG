using Entity.Abilities.FlameWitch;
using Framework.Core;
using Framework.Core.Modules;
using Modules.Teamable;
using UnityEngine;

namespace Framework.Ability
{
    public class CommonAbilityInfo
    {
        private Actor _actor;
        private Transform _transform;
        private ITeamable _teamable;
        private IMagicable _magicable;
        private IAbilitiable _abilitiable;
        public IMagicable Magicable => _magicable;

        public ITeamable Teamable => _teamable;

        public IAbilitiable Abilitiable => _abilitiable;
        public Transform Transform => _transform;
        public float Range { get; set; }

        public CommonAbilityInfo(Actor actor)
        {
            _magicable = actor.GetComponent<IMagicable>();
            _teamable = actor.GetComponent<ITeamable>();
            _abilitiable = actor.GetComponent<IAbilitiable>();
            _transform = actor.transform;
            _actor = actor;
        }

        public float ManaCost { get; set; }

        public bool TrySpendMana()
        {
            if (HasMana())
            {
                SpendMana();
                return true;
            }

            return false;
        }

        public bool HasMana() => _magicable.HasMagic(ManaCost);

        public void SpendMana() => _magicable.SpendMagic(ManaCost);

        public bool SameTeam(GameObject go, bool defaultValue = false) =>
            SameTeam(go.GetComponent<ITeamable>(), defaultValue);

        public bool SameTeam(ITeamable teamable, bool defaultValue = false)
        {
            if (_teamable == null || teamable == null)
                return defaultValue;
            return _teamable.SameTeam(teamable);
        }

        public bool InRange(Transform transform) => AbilityHelper.InRange(_transform, transform.position, Range);
        public bool InRange(Vector3 position) => AbilityHelper.InRange(_transform, position, Range);


        public void NotifySpellCast(SpellEventArgs args) => _abilitiable.NotifySpellCast(args);

        public void NotifySpellCast() =>
            _abilitiable.NotifySpellCast(new SpellEventArgs() {Caster = _actor, ManaSpent = ManaCost});
    }


    public class AbilityObject : ScriptableObject, IAbility, IAbilityView
    {
#pragma warning disable 0649

        [SerializeField] private Sprite _icon;
        protected CommonAbilityInfo _commonAbilityInfo;
#pragma warning disable 0649

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