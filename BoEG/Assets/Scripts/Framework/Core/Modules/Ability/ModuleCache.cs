using UnityEngine;

namespace MobaGame.Framework.Core.Modules.Ability
{
    public class ModuleCache : ComponentCache
    {
        //Helper functions
        public IHealthable Healthable => GetCached<IHealthable>();
        public IMagicable Magicable => GetCached<IMagicable>();
        public IAbilitiable Abilitiable => GetCached<IAbilitiable>();
        public ITeamable Teamable => GetCached<ITeamable>();

        public IMovable Movable => GetCached<IMovable>();

        public IAttackerable Attackerable => GetCached<IAttackerable>();

        public ICommandable Commandable => GetCached<ICommandable>();

        public IModifiable Modifiable => GetCached<IModifiable>();

        public IKillable Killable => GetCached<IKillable>();

        public ModuleCache(GameObject gameObject) : base(gameObject)
        {
        }

        public ModuleCache(Transform transform) : base(transform)
        {
        }
    }
}