using System.Collections.Generic;
using System.Linq;
using Components.Buffable;
using Core;

namespace Components
{
    public class BuffedModule : Module
    {
        private IBuffableInstance _buffable;

        public override void Initialize(Entity e)
        {
            _buffable = e.GetComponent<IBuffableInstance>();
            
        }

        public IEnumerable<T> GetBuffs<T>() where T : IBuffInstance
        {
            return (_buffable != null) ? _buffable.GetBuffs<T>() : Enumerable.Empty<T>();
        }
    }
}