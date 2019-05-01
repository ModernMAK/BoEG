using System;
using Framework.Types;

namespace DeleteMe
{
    public class DamageTarget : IDamageTarget
    {
        public virtual void TakeDamage(Damage damage)
        {
            var args = new DamageEventArgs(damage);
            OnDamaging(args);
            Callback(damage);
            OnDamaged(args);            
        }
        
        protected Action<Damage> Callback { get; set; }


        protected virtual void OnDamaged(DamageEventArgs e)
        {
            Damaged?.Invoke(this, e);
        }

        protected virtual void OnDamaging(DamageEventArgs e)
        {
            Damaging?.Invoke(this, e);
        }
        
        public event EventHandler<DamageEventArgs> Damaged;
        public event EventHandler<DamageEventArgs> Damaging;
    }
    public interface IDamageTarget
    {
        void TakeDamage(Damage damage);
        event EventHandler<DamageEventArgs> Damaged;
        event EventHandler<DamageEventArgs> Damaging;
    }
    public class DamageEventArgs : EventArgs
    {
        public DamageEventArgs(Damage damage)
        {
            Damage = damage;
        }

        public Damage Damage { get; private set; }
    }
}