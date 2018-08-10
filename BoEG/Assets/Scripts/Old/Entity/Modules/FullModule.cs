using System.Collections.Generic;
using System.Linq;
using Old.Entity.Modules.Buffable;
using Old.Entity.Modules.Levelable;

namespace Old.Entity.Modules
{
    public class FullModule : Module
    {
        private IBuffable _buffable;
        private ILevelable _levelable;

        protected override void Initialize()
        {
            _levelable = GetComponent<ILevelable>();
            _buffable = GetComponent<IBuffable>();
        }

        public int GetLevel(int defaultValue = 1)
        {
            return (_levelable != null) ? _levelable.Level : defaultValue;            
        }
        public IEnumerable<T> GetBuffs<T>() where T: IBuffInstance
        {
            return (_buffable != null) ? _buffable.GetBuffs<T>() : Enumerable.Empty<T>();
        }

    }
}