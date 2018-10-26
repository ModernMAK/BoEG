using System;
using Modules.Abilityable;
using Modules.Attackerable;
using Modules.Healthable;
using Modules.Teamable;
using UnityEngine;
using Util;

namespace Entity.Abilities.DarkHeart
{
    [CreateAssetMenu(fileName = "DarkHeart_Fearmonger.asset", menuName = "Ability/DarkHeart/Fearmonger")]
    public class Fearmonger : BetterAbility
    {
        private Nightmare _nightmareAbility;
        private IAttackable _attackable;
        private IHealthable _healthable;
        private ITeamable _teamable;
        private DynamicProc _proc;


        [SerializeField] [Range(0f, 1f)] private float _minChance;
        [SerializeField] [Range(0f, 1f)] private float _maxChance;
        [SerializeField] [Range(0f, 1f)] private float _selfHealthWeight;

        protected override int MaxLevel
        {
            get { return 1; }
        }

        public override void Initialize(GameObject go)
        {
            base.Initialize(go);
            Level = 1;//This ability is 'innate' and starts off being leveled
            _nightmareAbility = go.GetComponent<IAbilitiable>().GetAbility<Nightmare>();
            _attackable = go.GetComponent<IAttackable>();
            _teamable = go.GetComponent<ITeamable>();
            _healthable = go.GetComponent<IHealthable>();
            _proc = new DynamicProc(0f);

            _attackable.IncomingAttackLanded += IncomingAttackLanded;
        }

        private float CalculateChance(IHealthable self, IHealthable target)
        {
            return self.HealthPercentage * _selfHealthWeight +
                   target.HealthPercentage * (1f - _selfHealthWeight);
        }

        private void Apply(GameObject go, IHealthable healthable)
        {
            if (healthable == null)
                throw new Exception();

            if (_proc.Proc(CalculateChance(_healthable, healthable)))
                _nightmareAbility.UnitCast(go);
        }


        private void IncomingAttackLanded(DamageEventArgs args)
        {
            var target = args.Source;
            var healthable = target.GetComponent<IHealthable>();

            Apply(target, healthable);
        }

        public override void Terminate()
        {
            _attackable.IncomingAttackLanded -= IncomingAttackLanded;
        }


    }
}