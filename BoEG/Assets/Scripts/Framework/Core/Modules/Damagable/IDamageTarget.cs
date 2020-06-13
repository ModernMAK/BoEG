using System;
using Framework.Types;
using UnityEngine;

namespace Framework.Core.Modules
{
    public interface IDamageTarget
    {
        [Obsolete("Use overload and provide a source")]
        void TakeDamage(Damage damage);
        void TakeDamage(GameObject gameObject, Damage damage);
        event EventHandler<DamageEventArgs> Damaged;
        event EventHandler<DamageEventArgs> Damaging;
    }
}