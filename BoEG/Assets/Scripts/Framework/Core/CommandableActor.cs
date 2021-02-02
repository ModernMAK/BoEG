using System.Collections.Generic;
using Framework.Core;
using MobaGame.Framework.Core.Modules;

namespace MobaGame.Framework.Core
{
    public class CommandableActor : Actor, IProxy<ICommandable>, IRespawnable
    {
        private Commandable _commandable;
        ICommandable IProxy<ICommandable>.Value => _commandable;

        protected override void CreateComponents()
        {
            base.CreateComponents();
            _commandable = new Commandable(this);
        }

        protected override void Awake()
        {
            base.Awake();
        }


        protected override IEnumerable<object> Modules
        {
            get
            {
                foreach (var m in base.Modules)
                    yield return m;
                yield return _commandable;
            }
        }

        public void Respawn()
        {
            var modules = GetModules<IRespawnable>();
            foreach (var respawnable in modules)
                respawnable.Respawn();
        }
    }
}