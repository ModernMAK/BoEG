using System;
using System.Collections.Generic;

namespace MobaGame.Framework.Core.Modules
{
    public interface IAggroable
    {
        /// <summary>
        /// The range to search for aggro targets.
        /// </summary>
        /// <remarks>Should always be positive or 0.</remarks>
        float SearchRange { get; }
        /// <summary>
        /// Exposes the list of targets we can aggro.
        /// </summary>
        IReadOnlyList<Actor> Targets { get; }

        [Obsolete]
        bool HasTarget { get; }
    }

    public static class AggroableX
    {
        public static bool HasTarget(this IAggroable aggroable) => aggroable.Targets.Count > 0;
    }
}