using System;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public class Damageable : KillableActorModule, IDamageable
    {
        private readonly IArmorable _armorable;
        private readonly IHealthable _healthable;
        
        public Damageable(Actor actor, IHealthable healthable, IKillable killable, IArmorable armorable = default) : base(actor, killable)
        {
            _healthable = healthable;
            _armorable = armorable;
        }

        public virtual bool TakeDamage(SourcedDamage damage)
        {
            //ARMORABLE
            if (_armorable != null) damage = _armorable.ResistDamage(damage);
            damage = CalculateDamageModifiers(damage);
            OnDamaging(damage);
            _healthable.Value -= damage.Value.Value;
            OnDamaged(damage);
            var killed = _healthable.Value == 0f;
            if (killed)
                Killable.Die(damage.Source);
            return killed;
        }


        public event EventHandler<SourcedDamage> Damaged;
        public event EventHandler<SourcedDamage> Damaging;
        /// <remarks>
        /// This is calculated after Armor reduction
        /// </remarks>
        public event EventHandler<ChangableEventArgs<SourcedDamage>> DamageMitigation;




        protected virtual void OnDamaged(SourcedDamage e) => Damaged?.Invoke(this, e);      
        protected virtual void OnDamaging(SourcedDamage e) => Damaging?.Invoke(this, e);
        protected SourcedDamage CalculateDamageModifiers(SourcedDamage e) => DamageMitigation.CalculateChange(this,e);
        
    }
}