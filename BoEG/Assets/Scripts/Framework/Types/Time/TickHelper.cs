using UnityEngine;

namespace MobaGame.Framework.Types
{
    public class TickHelper
    {
        
        public int TickCount { get; set; }
        public int TicksPerformed { get; set; }
        public int TicksLeft => TickCount - TicksPerformed;
        public bool IsDone => TicksLeft <= 0;
        public float TickInterval { get; set; }
        public float ElapsedTime { get; set; }
        public float TotalElapsedTime => ElapsedTime + TicksPerformed * TickInterval;
        public float TotalTime => TickCount * TickInterval;
        public float TimeNormal => TotalElapsedTime / TotalTime;


        /// <summary>
        ///     Advances tick time, and returns the number of ticks to perform.
        ///     Returns true if there are no more ticks to perform
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public virtual bool Advance(float deltaTime, out int ticks)
        {
            ElapsedTime += deltaTime;
            var ticksRequested = Mathf.FloorToInt(ElapsedTime / TickInterval);
            ElapsedTime -= TickInterval * ticksRequested; //Doesnt matter if we fix before or after
            TicksPerformed += ticksRequested;
            if (ticksRequested > TicksLeft) ticksRequested = TicksLeft;

            ticks = ticksRequested;
            return IsDone;
        }
    }
}