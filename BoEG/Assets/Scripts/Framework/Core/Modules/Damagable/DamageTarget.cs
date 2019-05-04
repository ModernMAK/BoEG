using System;
using Framework.Types;

namespace Framework.Core.Modules
{
    public class DamageTarget : IDamageTarget
    {
        public DamageTarget(IArmorable armorable, IHealthable healthable)
        {
            _armorable = armorable;
            _healthable = healthable;
        }

        private readonly IArmorable _armorable;
        private readonly IHealthable _healthable;
        
        public virtual void TakeDamage(Damage damage)
        {
//            OnDamaging(args);
//            Callback(damage);
//            OnDamaged(args); 
            
            
            var damageToTake = damage;
            //ARMORABLE
            if (_armorable != null)
            {
                damageToTake = _armorable.ResistDamage(damage);
            }
            OnDamaging(damageToTake);
            _healthable.ModifyHealth(damageToTake.Value);
            OnDamaged(damageToTake);
        }
        
//        protected Action<Damage> Callback { get; set; }


        private void OnDamaged(Damage damage)
        {
            OnDamaged(new DamageEventArgs(damage));
        }
        protected virtual void OnDamaged(DamageEventArgs e)
        {
            Damaged?.Invoke(this, e);
        }

        private void OnDamaging(Damage damage)
        {
            OnDamaging(new DamageEventArgs(damage));
        }
        protected virtual void OnDamaging(DamageEventArgs e)
        {
            Damaging?.Invoke(this, e);
        }
        
        public event EventHandler<DamageEventArgs> Damaged;
        public event EventHandler<DamageEventArgs> Damaging;
    }
}