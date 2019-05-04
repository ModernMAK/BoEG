using System;
using Framework.Types;
using UnityEngine;

namespace Framework.Core.Modules
{
    public class DamageTargetComponent : MonoBehaviour, IComponent<IDamageTarget>, IDamageTarget
    {
        private IDamageTarget _damageTarget;
        public void TakeDamage(Damage damage)
        {
            _damageTarget.TakeDamage(damage);
        }

        public event EventHandler<DamageEventArgs> Damaged
        {
            add => _damageTarget.Damaged += value;
            remove => _damageTarget.Damaged -= value;
        }

        public event EventHandler<DamageEventArgs> Damaging
        {
            add => _damageTarget.Damaging += value;
            remove => _damageTarget.Damaging -= value;
        }

        public void Initialize(IDamageTarget module)
        {
            _damageTarget = module;
        }
    }
}