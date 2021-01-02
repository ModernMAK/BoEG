using Framework.Core;
using Framework.Core.Modules;
using Modules.Teamable;
using UnityEngine;

namespace Framework.Ability
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
}