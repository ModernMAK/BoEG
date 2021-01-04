using System;

namespace MobaGame.Framework.Types
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
}