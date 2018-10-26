using System;
using Core;
using UnityEngine;
using Util;

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
            var attackable = go.GetComponent<IAttackable>();
            //Tell the attackable we are attacking it
            attackable.TargetForAttack(Self);
            //Tell the attackable we attacked it - This will be necessary when we impliment evasion and whatnot
            //TODO impliment evasion in attackable
            attackable.RecieveAttack(damage);
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
            if (OutgoingAttackLanded != null)
                OutgoingAttackLanded();
        }

        public event DEFAULT_HANDLER AttackLaunched;
        public event DEFAULT_HANDLER OutgoingAttackLanded;
    }
}