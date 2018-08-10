using System;
using Core;
using Old.Entity.Modules.Healthable;
using UnityEngine;

namespace Old.Entity.Modules.Attackerable
{
    [Serializable]
    public class Attackerable : FullModule, IAttackerable
    {
        protected override void Initialize()
        {
            _data = GetData<IAttackerableData>();
        }

        /// <summary>
        /// The backing field containing template data for Attackerable.
        /// </summary>
        [SerializeField] private IAttackerableData _data;


        public float Damage
        {
            get
            {
                var baseVal = _data.BaseDamage;
                var gainRate = _data.GainDamage;
                Func<IAttackerableBuffInstance, float> bonusFunc = (x => x.DamageBonus);
                Func<IAttackerableBuffInstance, float> multFunc = (x => x.DamageMultiplier);
                return this.CalculateValue(baseVal, gainRate, bonusFunc, multFunc, true);
            }
        }

        public float AttackRange
        {
            get
            {
                var baseVal = _data.BaseAttackRange;
                var gainRate = 0f;//There s no gain rate
                Func<IAttackerableBuffInstance, float> bonusFunc = (x => x.AttackRangeBonus);

                return this.CalculateValueBonus(baseVal,gainRate,  bonusFunc, true);
            }
        }

        public float AttackSpeed
        {
            get
            {
                var baseVal = _data.BaseAttackSpeed;
                var gainRate = _data.GainAttackSpeed;
                Func<IAttackerableBuffInstance, float> bonusFunc = (x => x.AttackSpeedBonus);

                return this.CalculateValueBonus(baseVal, gainRate, bonusFunc, true);
            }
        }

        public GameObject Projectile
        {
            get { return _data.Projectile; }
        }


        private void MeleeAttack(GameObject target)
        {
            var damage = new Damage(Damage, DamageType.Physical, gameObject);
            var args = new AttackEventArgs(target, damage);
            OnAttackLaunched(args);
            var targetHealthable = target.GetComponent<IHealthable>();
            if (targetHealthable != null)
                targetHealthable.TakeDamage(damage);
        }

        public void Attack(GameObject go)
        {
            //Melee
            if (Projectile == null)
            {
                MeleeAttack(go);
            }
            //Ranged
            else
            {
                throw new Exception();
            }
        }


        public event AttackHandler AttackLaunched;

        protected void OnAttackLaunched(AttackEventArgs attackArgs)
        {
            if (AttackLaunched != null)
                AttackLaunched(attackArgs);
        }

        public event AttackHandler AttackLanded;

        protected void OnAttackLanded(AttackEventArgs attackArgs)
        {
            if (AttackLanded != null)
                AttackLanded(attackArgs);
        }

        public event AttackHandler AttackKilled;

        protected void OnAttackKilled(AttackEventArgs attackArgs)
        {
            if (AttackKilled != null)
                AttackKilled(attackArgs);
        }
    }

    public delegate void AttackHandler(AttackEventArgs attackArgs);

    public class AttackEventArgs : System.EventArgs
    {
        public AttackEventArgs(GameObject target, Damage damage)
        {
            Target = target;
            Damage = damage;
        }

        public GameObject Source
        {
            get { return Damage.Source; }
        }

        public GameObject Target { get; private set; }
        public Damage Damage { get; private set; }
    };
}