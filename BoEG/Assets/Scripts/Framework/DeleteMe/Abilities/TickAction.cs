using UnityEngine;

namespace Entity.Abilities
{
    public abstract class TickAction
    {
        protected TickAction(TickData data)
        {
            Data = data;
            Performed = 0;
            ElapsedTime = 0f;
        }

        protected TickData Data { get; }

        protected float Duration => Data.Duration;

        protected float Interval => Data.Interval;

        protected int Performed { get; private set; }


        protected float ElapsedTime { get; private set; }

        protected int TicksRemaining => Mathf.FloorToInt(Duration - Interval * Performed / Interval);

        protected int TicksToPerform
        {
            get
            {
                var performing = Mathf.FloorToInt(ElapsedTime / Interval);
                return Mathf.Min(performing, TicksRemaining);
            }
        }

        public bool DoneTicking => TicksRemaining <= 0;

        protected abstract void Logic();

        public void Tick(float time)
        {
            AdvanceTime(time);
            Logic();
            AdvanceTicks();
        }

        public virtual void Terminate()
        {
        }

        private void AdvanceTime(float time)
        {
            ElapsedTime += time;
        }


        private void AdvanceTicks()
        {
            var temp = TicksToPerform;
            Performed += temp;
            ElapsedTime -= Interval * temp;
        }
    }
}