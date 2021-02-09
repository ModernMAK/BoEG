using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Ability
{
    public class CommonAbilityInfo
    {
        private readonly Actor _actor;
        private readonly Transform _transform;
        private readonly ITeamable _teamable;
        private readonly IMagicable _magicable;
        private readonly IAbilitiable _abilitiable;
        public IMagicable Magicable => _magicable;

        public ITeamable Teamable => _teamable;

        public IAbilitiable Abilitiable => _abilitiable;
        public Transform Transform => _transform;
        public float Range { get; set; }

        public CommonAbilityInfo(Actor actor)
        {
            _magicable = actor.GetModule<IMagicable>();
            _teamable = actor.GetModule<ITeamable>();
            _abilitiable = actor.GetModule<IAbilitiable>();
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


        public bool InRange(Transform transform) => AbilityHelper.InRange(_transform, transform.position, Range);
        public bool InRange(Vector3 position) => AbilityHelper.InRange(_transform, position, Range);


        public void NotifySpellCast(AbilityEventArgs args) => _abilitiable.NotifyAbilityCast(args);

        public void NotifySpellCast() =>
            _abilitiable.NotifyAbilityCast(new AbilityEventArgs( _actor,  ManaCost));
    }
}