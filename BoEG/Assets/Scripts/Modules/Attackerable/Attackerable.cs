using System;
using Core;
using Modules.Healthable;
using Old;
using UnityEngine;

namespace Modules.Attackerable
{
    [Serializable]
    public class Attackerable : Module, IAttackerable
    {
        private readonly IAttackerableData _data;
        private float _lastAttackTime;

        public Attackerable(GameObject self, IAttackerableData data) : base(self)
        {
            _data = data;
            _lastAttackTime = Mathf.NegativeInfinity;
        }

        public float AttackDamage
        {
            get { return _data.AttackDamage.Evaluate(); }
        }

        public float AttackRange
        {
            get { return _data.AttackRange; }
        }

        public float AttackSpeed
        {
            get { return _data.AttackSpeed.Evaluate(); }
        }

        public GameObject Projectile
        {
            get { return _data.Projectile; }
        }


        public void Attack(GameObject go)
        {
            //Ignore call if we cant
            if (_lastAttackTime + AttackSpeed > Time.time)
                return;

            if (Projectile == null)
                MeleeAttack(go);
            else
                RangedAttack(go);
        }

        private void MeleeAttack(GameObject go)
        {
            _lastAttackTime = Time.time;
            var damage = new Damage(AttackDamage, DamageType.Physical, Self);
            var healthable = go.GetComponent<IHealthable>();
            healthable.TakeDamage(damage);
        }

        private void RangedAttack(GameObject go)
        {
            _lastAttackTime = Time.time;
        }

        private void OnAttackLaunched()
        {
            if (AttackLaunched != null)
                AttackLaunched();
        }

        private void OnAttackLanded()
        {
            if (AttackLanded != null)
                AttackLanded();
        }

        public event DEFAULT_HANDLER AttackLaunched;
        public event DEFAULT_HANDLER AttackLanded;
    }
}