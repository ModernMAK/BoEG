using Modules.Abilityable;
using Modules.Attackerable;
using Modules.Healthable;
using Modules.Teamable;
using UnityEngine;
using Util;

namespace Entity.Abilities.DarkHeart
{
    [CreateAssetMenu(fileName = "DarkHeart_Fearmonger.asset", menuName = "Ability/DarkHeart/Fearmonger")]
    public class Fearmonger : Ability
    {
        private Nightmare _nightmareAbility;

        private IAttackable _attackable;
        private IHealthable _healthable;
        private ITeamable _teamable;
        private ConstantProc _proc;
        [SerializeField] [Range(0f, 1f)] private float _minChance;
        [SerializeField] [Range(0f, 1f)] private float _maxChance;
        [SerializeField] [Range(0f, 1f)] private float _selfHealthWeight;
        private float EnemyHealthWeight
        {
            get { return 1f - _selfHealthWeight; }
        }


        public override void Initialize(GameObject go)
        {
            _nightmareAbility = go.GetComponent<IAbilitiable>().GetAbility<Nightmare>();
            _attackable = go.GetComponent<IAttackable>();
            _teamable = go.GetComponent<ITeamable>();
            _healthable = go.GetComponent<IHealthable>();
            _proc = new ConstantProc(0f);

            _attackable.IncomingAttackLanded += IncomingAttackLanded;
        }


        private void IncomingAttackLanded(DamageEventArgs args)
        {
            var source = args.Source;
            var healthable = source.GetComponent<IHealthable>();

            var weight = 0f;
            var totalWeight = _selfHealthWeight + EnemyHealthWeight;

            if (_healthable == null)
                totalWeight -= _selfHealthWeight;
            else
                weight += _healthable.HealthPercentage * _selfHealthWeight;

            if (healthable == null)
                totalWeight -= EnemyHealthWeight;
            else
                weight += healthable.HealthPercentage * EnemyHealthWeight;

            var chance = (_minChance + _maxChance) / 2f;
            if (!totalWeight.SafeEquals(0f))
                chance = (weight / totalWeight) * (_maxChance - _minChance) + _minChance;

            _proc.SetChace(chance);
            if (_proc.Proc())
            {
                _nightmareAbility.ApplyNightmare(source);
            }
        }

        public override void Terminate()
        {
            _attackable.IncomingAttackLanded -= IncomingAttackLanded;
        }

        public override void Trigger()
        {
        }

        //TODO impliment a function to act as an attack reciever (IAttackerable acts as an attack launcher)
        //How would this delegate to Healthable? Or Armorable?
    }
}