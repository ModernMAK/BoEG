namespace Framework.Types
{
    public class DurationTimer
    {
        public DurationTimer(float duration)
        {
            Duration = duration;
        }

        public float ElapsedTime { get; set; }
        public float Duration { get; }
        public float RemainingTime => Duration - ElapsedTime;
        public bool Done => RemainingTime <= 0f;


        public void Reset() => ElapsedTime = 0;

        public bool AdvanceTime(float deltaTime)
        {
            ElapsedTime += deltaTime;
            return Done;
        }
    }
}