using Framework.Types;

namespace Framework.Core.Modules
{
    public interface IDamagable
    {
        void TakeDamage(Damage damage);
        event DamageEventHandler DamageTaking;
        event DamageEventHandler DamageTaken;

    }
}