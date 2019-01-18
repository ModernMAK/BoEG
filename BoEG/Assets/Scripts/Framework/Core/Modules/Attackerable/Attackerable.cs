using System;
using System.Collections.Generic;
using Framework.Types;
using UnityEngine;
using UnityEngine.Analytics;

namespace Framework.Core.Modules
{
    public class Attackerable : Module, IAttackerable
    {
        private static Collider[] _PhysicsBuffer = new Collider[256];
        private IAttackerableData _data;
        protected override void Instantiate()
        {
            base.Instantiate();
            GetData(out _data);
            _attackTargets = new List<GameObject>();
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
        private List<GameObject> _attackTargets;

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
        
        public IEnumerable<GameObject> GetAttackTargets()
        {
            return _attackTargets;
        }

        public bool HasAttackTarget()
        {
            return _attackTargets.Count >0;
        }

        /// <summary>
        /// NOT THREAD SAFE
        /// </summary>
        /// <param name="deltaStep"></param>
        protected override void PhysicsStep(float deltaStep)
        {
            var size = Physics.OverlapSphereNonAlloc(transform.position, AttackRange, _PhysicsBuffer);
            _attackTargets.Clear();
            for (var i = 0; i < size; i++)
                _attackTargets.Add(_PhysicsBuffer[i].gameObject);
            _attackTargets.Sort((x, y) =>
            {
                var pos = transform.position;
                var xDelta = (x.transform.position - pos);
                var yDelta = (y.transform.position - pos);
                return xDelta.sqrMagnitude.CompareTo(yDelta.sqrMagnitude);
            });
        }
    }
}