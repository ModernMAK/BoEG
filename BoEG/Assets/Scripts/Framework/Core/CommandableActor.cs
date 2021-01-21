using System.Collections.Generic;
using Framework.Core;
using MobaGame.Framework.Core.Modules;

namespace MobaGame.Framework.Core
{
    public class CommandableActor : Actor, IProxy<ICommandable>, IRespawnable
    {
        private Commandable _commandable;
        private IEnumerable<IRespawnable> _respawnables;
        ICommandable IProxy<ICommandable>.Value => _commandable;

        protected override void CreateComponents()
        {
            base.CreateComponents();
            _commandable = new Commandable(this);
        }

        protected override void Awake()
        {
            base.Awake();
            _respawnables = GetModules<IRespawnable>();
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
            foreach (var respawnable in _respawnables)
                respawnable.Respawn();
        }
    }
}