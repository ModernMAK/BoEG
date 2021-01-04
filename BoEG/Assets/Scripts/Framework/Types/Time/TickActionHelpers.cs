using System.Collections.Generic;
using System.Linq;

namespace MobaGame.Framework.Types
{
    public static class TickActionHelpers
    {
        public static void AdvanceAllAndRemoveDone(this ICollection<TickAction> ticks, float deltaTime)
        {
            var temp = ticks.Where(tick => tick.Advance(deltaTime)).ToList();

            foreach (var t in temp)
            {
                ticks.Remove(t);
            }
        }

        public static void AdvanceAllAndRemoveDone(this IList<TickAction> ticks, float deltaTime)
        {
            for (var i = 0; i < ticks.Count; i++)

                if (ticks[i].Advance(deltaTime))
                {
                    ticks.RemoveAt(i);
                    i--;
                }
        }
    }
}