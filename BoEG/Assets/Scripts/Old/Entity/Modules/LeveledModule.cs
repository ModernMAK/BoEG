using Old.Entity.Modules.Levelable;

namespace Old.Entity.Modules
{
    public class LeveledModule : Module
    {
        private ILevelable _levelable;

        protected override void Initialize()
        {
            _levelable = GetComponent<ILevelable>();
        }

        public int GetLevel(int defaultValue = 1)
        {
            return (_levelable != null) ? _levelable.Level : defaultValue;
            
        }
    }
}