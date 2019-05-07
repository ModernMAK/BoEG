using System;
using Framework.Types;

namespace Framework.Core.Modules
{
    [Obsolete("Use IStepable, ISpawnable, & IInstantiable separately")]
    public interface IModule : IStepable, ISpawnable, IInstantiable
    {
    }
}