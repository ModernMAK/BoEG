using System;
using MobaGame.Framework.Logging;
using MobaGame.Framework.Types;
using MobaGame.Framework.Utility;
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
            DamageableLog.LogDamaging(damage, Actor);
            OnDamaging(damage);
            //ARMORABLE
            if (_armorable != null) damage = _armorable.ResistDamage(damage);
            damage = CalculateDamageModifiers(damage);
            _healthable.Value -= damage.Value.Value;
            DamageableLog.LogDamaged(damage, Actor);
            OnDamaged(damage);
            var killed = _healthable.Value.SafeEquals(0f);
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
    public static class DamageableLog
    {
        
        public static void LogDamaging(SourcedDamage damage, Actor actor)
        {
            var msg =
                $"{CombatLog.FormatActor(damage.Source.name)} Damaging {CombatLog.FormatActor(actor.name)} for {CombatLog.FormatDamage(damage.Value)}";
            CombatLog.Log(msg);
        }

        public static void LogDamaged(SourcedDamage damage, Actor actor)
        {
            var msg =
                $"{CombatLog.FormatActor(damage.Source.name)} Damaged {CombatLog.FormatActor(actor.name)} for {CombatLog.FormatDamage(damage.Value)}";
            CombatLog.Log(msg);
        }
    }
}