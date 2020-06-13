using System;

namespace Entity.Abilities.FlameWitch
{
    public class TickAction : TickHelper
    {
        public Action Callback { get; set; }

        public virtual bool Advance(float deltaTime)
        {
            var result = Advance(deltaTime, out var ticks);
            for (var i = 0; i < ticks; i++)
                Callback();
            return result;
        }
    }

    public class InfiniteTickAction : TickAction
    {
        public override bool Advance(float deltaTime, out int ticks)
        {
            TicksPerformed = 0;
            TickCount = 100;
            base.Advance(deltaTime, out ticks);
            return false;
        }
    }
}