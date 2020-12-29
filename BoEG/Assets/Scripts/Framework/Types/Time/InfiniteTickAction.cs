namespace Framework.Types
{
    public class InfiniteTickAction : TickAction
    {
        public override bool Advance(float deltaTime, out int ticks)
        {
            TicksPerformed = 0;
            TickCount = int.MaxValue; //If we have more ticks than this, we've got bigger problems
            base.Advance(deltaTime, out ticks);
            return false;
        }
    }
}