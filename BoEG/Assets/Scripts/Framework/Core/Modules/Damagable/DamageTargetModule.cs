using System;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public class DamageTarget : ActorModule, IDamageTarget
    {
        private IArmorable _armorable;
        private IHealthable _healthable;
        private IKillable _killable;
        private event EventHandler<DamageEventArgs> _damaged;
        private event EventHandler<DamageEventArgs> _damaging;

        public DamageTarget(Actor actor, IHealthable healthable, IKillable killable, IArmorable armorable = default) : base(actor)
        {
            _healthable = healthable;
            _armorable = armorable;
            _killable = killable;
        }

        // _armorable = GetComponent<IArmorable>();
        // _healthable = GetComponent<IHealthable>();

        public virtual bool TakeDamage(Actor source, Damage damage)
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
            var killed = _healthable.Health == 0f;
            if (killed)
                _killable.Die(dmgArg.Source);
            return killed;
        }

        public bool TakeDamage(SourcedDamage<Actor> damage) => TakeDamage(damage.Source, damage.Damage);

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