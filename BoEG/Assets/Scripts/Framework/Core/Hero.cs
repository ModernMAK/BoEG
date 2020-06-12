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
//            var buffable = new Buffable();
//            GetFrameworkComponent<IBuffable>().Initialize(buffable);

            GetFrameworkComponent<IHealthableData>().Initialize(_healthableData);
            
            GetFrameworkComponent<IMagicableData>().Initialize(_magicableData);

            GetFrameworkComponent<IArmorableData>().Initialize(_armorableData);
            
//            GetFrameworkComponent<IDamageTarget>().Initialize();

            GetFrameworkComponent<IAttackerableData>().Initialize(_attackerableData);

            GetFrameworkComponent<IMovableData>().Initialize(_movableData);
        }

        [SerializeField] private HealthableData _healthableData;
        [SerializeField] private MagicableData _magicableData;
        [SerializeField] private ArmorableData _armorableData;
        [SerializeField] private AttackerableData _attackerableData;
        [SerializeField] private MovableData _movableData;
    }
}