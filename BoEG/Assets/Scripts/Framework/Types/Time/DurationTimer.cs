namespace MobaGame.Framework.Types
{
    public class DurationTimer
    {
        public DurationTimer(float duration, bool startDone = false)
        {
            Duration = duration;
            ElapsedTime = startDone ? Duration : 0f;
        }

        public float ElapsedTime { get; set; }
        public float Duration { get; set; }
        public float RemainingTime => Duration - ElapsedTime;
        public bool Done => RemainingTime <= 0f;


        public void Reset() => ElapsedTime = 0;
        public void SetDone() => ElapsedTime = Duration;

        public bool AdvanceTime(float deltaTime)
        {
            ElapsedTime += deltaTime;
            return Done;
        }

        public bool AdvanceTimeIfNotDone(float deltaTime) => Done || AdvanceTime(deltaTime);
    }
}