using System;
using Framework.Core;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public class DamageTarget : ActorModule, IDamageTarget
    {
        private IArmorable _armorable;
        private IHealthable _healthable;
        private event EventHandler<DamageEventArgs> _damaged;
        private event EventHandler<DamageEventArgs> _damaging;

        public DamageTarget(Actor actor, IHealthable healthable, IArmorable armorable = default) : base(actor)
        {
            _healthable = healthable;
            _armorable = armorable;
        }

        // _armorable = GetComponent<IArmorable>();
        // _healthable = GetComponent<IHealthable>();

        public virtual void TakeDamage(GameObject source, Damage damage)
        {
            var damageToTake = damage;
            //ARMORABLE
            if (_armorable != null) damageToTake = _armorable.ResistDamage(damage);

            var dmgArg = new DamageEventArgs
            {
                Damage = damageToTake,
                Source = source
            };
            OnDamaging(dmgArg);
            _healthable.Health -= dmgArg.Damage.Value;
            OnDamaged(dmgArg);
        }

        public void TakeDamage(SourcedDamage<GameObject> damage) => TakeDamage(damage.Source, damage.Damage);

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


        protected virtual void OnDamaged(DamageEventArgs e)
        {
            _damaged?.Invoke(this, e);
        }

        protected virtual void OnDamaging(DamageEventArgs e)
        {
            _damaging?.Invoke(this, e);
        }
    }
}