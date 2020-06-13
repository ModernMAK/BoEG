using System;
using Framework.Types;
using UnityEngine;

namespace Framework.Core.Modules
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(IHealthable))]
    public class DamageTarget : MonoBehaviour, IDamageTarget
    {
        private void Awake()
        {
            _armorable = GetComponent<IArmorable>();
            _healthable = GetComponent<IHealthable>();
        }

        private IArmorable _armorable;
        private IHealthable _healthable;
        private event EventHandler<DamageEventArgs> _damaged;
        private event EventHandler<DamageEventArgs> _damaging;

        [Obsolete]
        public virtual void TakeDamage(Damage damage)
        {
        }

        public virtual void TakeDamage(GameObject source, Damage damage)
        {
            var damageToTake = damage;
            //ARMORABLE
            if (_armorable != null)
            {
                damageToTake = _armorable.ResistDamage(damage);
            }

            var dmgArg = new DamageEventArgs()
            {
                Damage = damageToTake,
                Source = source
            };
            OnDamaging(dmgArg);
            _healthable.Health -= dmgArg.Damage.Value;
            OnDamaged(dmgArg);
        }

        protected virtual void OnDamaged(DamageEventArgs e)
        {
            _damaged?.Invoke(this, e);
        }

        protected virtual void OnDamaging(DamageEventArgs e)
        {
            _damaging?.Invoke(this, e);
        }

        public event EventHandler<DamageEventArgs> Damaged
        {
            add => _damaged += value;
            remove => _damaged -= value;
        }

        public event EventHandler<DamageEventArgs> Damaging
        {
            add => _damaging += value;
            remove => _damaging -= value;
        }
    }
}