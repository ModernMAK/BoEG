using System;
using Framework.Types;
using UnityEngine;

namespace Framework.Core.Modules
{
    [DisallowMultipleComponent]
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

        public virtual void TakeDamage(Damage damage)
        {
            var damageToTake = damage;
            //ARMORABLE
            if (_armorable != null)
            {
                damageToTake = _armorable.ResistDamage(damage);
            }

            OnDamaging(new DamageEventArgs(damageToTake));
            _healthable.Health -= damageToTake.Value;
            OnDamaged(new DamageEventArgs(damageToTake));
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