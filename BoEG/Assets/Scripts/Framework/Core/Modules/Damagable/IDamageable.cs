using System;
using MobaGame.Framework.Types;
using UnityEngine;

namespace MobaGame.Framework.Core.Modules
{
    public interface IDamageable
    {

        bool TakeDamage(SourcedDamage damage);
        event EventHandler<SourcedDamage> Damaged;
        event EventHandler<SourcedDamage> Damaging;
        event EventHandler<ChangableEventArgs<SourcedDamage>> DamageMitigation;
    }
    public static class IDamageTargetX
	{
        public static bool TakeDamage(this IDamageable damageTarget, Actor actor, Damage damage) => damageTarget.TakeDamage(new SourcedDamage(actor, damage));
	}
}