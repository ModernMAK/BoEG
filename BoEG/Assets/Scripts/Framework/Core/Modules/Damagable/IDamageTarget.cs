using System;
using Framework.Types;

namespace Framework.Core.Modules
{
    public interface IDamageTarget
    {
        void TakeDamage(Damage damage);
        event EventHandler<DamageEventArgs> Damaged;
        event EventHandler<DamageEventArgs> Damaging;
    }
}