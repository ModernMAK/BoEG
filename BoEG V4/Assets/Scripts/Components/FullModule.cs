using System.Collections.Generic;
using System.Linq;
using Components.Buffable;
using Components.Levelable;
using Core;

namespace Components
{
    public class FullModule : Module
    {
        private IBuffableInstance _buffable;
        private ILevelableInstance _levelable;

        public override void Initialize(Entity e)
        {
            _levelable = e.GetComponent<ILevelableInstance>();
            _buffable = e.GetComponent<IBuffableInstance>();
        }

        public int GetLevel(int defaultValue = 1)
        {
            return (_levelable != null) ? _levelable.Level : defaultValue;            
        }
        public IEnumerable<T> GetBuffs<T>() where T : IBuffInstance
        {
            return (_buffable != null) ? _buffable.GetBuffs<T>() : Enumerable.Empty<T>();
        }

    }
}