using UnityEngine;

namespace Modules.MiscEvents
{
    public class KillEventArgs : System.EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source">The Killer</param>
        /// <param name="target">The Killed</param>
        public KillEventArgs(GameObject source, GameObject target)
        {
            Source = source;
            Target = target;
        }

        public GameObject Source { get; private set; }
        public GameObject Target { get; private set; }
    };
}