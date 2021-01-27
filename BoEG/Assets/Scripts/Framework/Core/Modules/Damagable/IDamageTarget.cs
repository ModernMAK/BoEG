using System;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public interface IDamageTarget
    {

        bool TakeDamage(Actor source, Damage damage);
        bool TakeDamage(SourcedDamage<Actor> damage);
        event EventHandler<DamageEventArgs> Damaged;
        event EventHandler<DamageEventArgs> Damaging;
    }
}