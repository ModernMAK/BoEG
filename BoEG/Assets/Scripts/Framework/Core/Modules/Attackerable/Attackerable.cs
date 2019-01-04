using System;
using Framework.Types;
using UnityEngine;
using UnityEngine.Analytics;

namespace Framework.Core.Modules
{
    public class Attackerable : Module, IAttackerable
    {
        private IAttackerableData _data;
        protected override void Instantiate()
        {
            base.Instantiate();
            GetData(out _data);
        }

        public float AttackRange
        {
            get { return IsInitialized ? _data.AttackRange : 0f; }
        }

        public float AttackDamage
        {
            get { return IsInitialized ? _data.AttackDamage : 0f; }
        }

        public float AttackSpeed
        {
            get { return IsInitialized ? _data.AttackSpeed : 0f; }
        }

        public bool IsRanged
        {
            get { return IsInitialized ? _data.IsRanged : false; }
        }

        private float _lastAttack = Mathf.NegativeInfinity;

        public bool CanAttack
        {
            get { return _lastAttack + AttackSpeed <= Time.time; }
        }

        private Damage CreateDamage()
        {
            return new Damage(AttackDamage, DamageType.Physical, gameObject);
        }

        public void Attack(GameObject target)
        {
            var targetable = target.GetComponent<ITargetable>();
            var damagable = target.GetComponent<IDamagable>();
            if (targetable == null || !CanAttack || !InRange(target) || damagable == null)
                return;

            Action callback = () => damagable.TakeDamage(CreateDamage());
            targetable.TargetAttack(gameObject, callback);
        }

        public bool InRange(GameObject target)
        {
            var deltaVector = transform.position - target.transform.position;
            return deltaVector.sqrMagnitude <= AttackRange * AttackRange;
        }
    }
}