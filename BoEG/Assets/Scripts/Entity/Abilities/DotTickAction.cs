using Core;
using Modules.Healthable;

namespace Entity.Abilities
{
    public abstract class DotTickAction : TickAction
    {
        protected DotTickAction(TickData data) : base(data)
        {
        }

        protected void ApplyDamageOverTime(IHealthable target, Damage damage)
        {
            var performingTicks = TicksToPerform;
            for (var i = 0; i < performingTicks; i++)
            {
                target.TakeDamage(damage);
            }
        }
    }
}