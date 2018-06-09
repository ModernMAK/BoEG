using Components.Levelable;
using Core;

namespace Components
{
    public class LeveledModule : Module
    {
        private ILevelable _levelable;

        public override void Initialize(Entity e)
        {
            _levelable = e.GetComponent<ILevelable>();
        }

        public int GetLevel(int defaultValue = 1)
        {
            return (_levelable != null) ? _levelable.Level : defaultValue;
            
        }
    }
}