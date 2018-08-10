using System.Collections.Generic;
using System.Linq;
using Old.Entity.Modules.Buffable;

namespace Old.Entity.Modules
{
    
    public class BuffedModule : Module
    {
        private IBuffable _buffable;

        protected override void Initialize()
        {
            _buffable = GetComponent<IBuffable>();
        }

        public IEnumerable<T> GetBuffs<T>() where T : IBuffInstance
        {
            return (_buffable != null) ? _buffable.GetBuffs<T>() : Enumerable.Empty<T>();
        }

    }
}