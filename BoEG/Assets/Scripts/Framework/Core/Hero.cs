using System;
using Framework.Core.Modules;
using UnityEngine;
using UnityEngine.AI;

namespace Framework.Core
{
    public class Hero : Actor
    {
        protected override void Awake()
        {
            base.Awake();
            SetupComponents();
        }
        

        private void SetupComponents()
        {
            //TODO register modules
            var healthable = new Healthable(_healthableData);
            var healthableModule = new HealthableModule(healthable);
            AddSteppable(healthableModule);
            GetFrameworkComponent<IHealthable>().Initialize(healthableModule);

            var magicable = new Magicable(_magicableData);
            var magicableModule = new MagicableModule(magicable);
            AddSteppable(magicableModule);
            GetFrameworkComponent<IMagicable>().Initialize(magicableModule);


            var armorable = new Armorable(_armorableData);
            GetFrameworkComponent<IArmorable>().Initialize(armorable);

            var damageTarget = new DamageTarget(armorable, healthable);
            GetFrameworkComponent<IDamageTarget>().Initialize(damageTarget);

            var attackerable = new Attackerable(_attackerableData);
            GetFrameworkComponent<IAttackerable>().Initialize(attackerable);
//            var magicable = new Magicable(_healthableData.HealthCapacity, _healthableData.HealthGeneration);
//            GetFrameworkComponent<IHealthable>().Initialize(healthable);

            var movable = new Movable(_movableData, GetComponent<NavMeshAgent>());
            GetFrameworkComponent<IMovable>().Initialize(movable);
        }

        [SerializeField] private HealthableData _healthableData;
        [SerializeField] private MagicableData _magicableData;
        [SerializeField] private ArmorableData _armorableData;
        [SerializeField] private AttackerableData _attackerableData;
        [SerializeField] private MovableData _movableData;
    }
}