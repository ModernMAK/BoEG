using Framework.Types;

namespace Framework.Core.Modules
{
    public class Damagable : Module, IDamagable
    {
        private IArmorable _armorable;
        private IHealthable _healthable;

        protected override void Instantiate()
        {
            base.Instantiate();
            _armorable = GetComponent<IArmorable>();
            _healthable = GetComponent<IHealthable>();
        }

        public void TakeDamage(Damage damage)
        {
            var damageToTake = damage;
            OnDamageTaking(damage);
            //ARMORABLE
            if (_armorable != null)
            {
                damageToTake = _armorable.ResistDamage(damage);
            }

            _healthable.ModifyHealth(damageToTake.Value);
            OnDamageTaken(damageToTake);
        }


        private void OnDamageTaking(Damage damage)
        {
            if (DamageTaking != null)
                DamageTaking(damage);
        }

        private void OnDamageTaken(Damage damage)
        {
            if (DamageTaken != null)
                DamageTaken(damage);
        }

        public event DamageEventHandler DamageTaking;
        public event DamageEventHandler DamageTaken;
    }
}