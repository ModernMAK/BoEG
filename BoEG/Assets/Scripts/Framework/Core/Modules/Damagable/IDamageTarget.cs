using System;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public interface IDamageTarget
    {

        void TakeDamage(GameObject gameObject, Damage damage);
        void TakeDamage(SourcedDamage<GameObject> damage);
        event EventHandler<DamageEventArgs> Damaged;
        event EventHandler<DamageEventArgs> Damaging;
    }
}