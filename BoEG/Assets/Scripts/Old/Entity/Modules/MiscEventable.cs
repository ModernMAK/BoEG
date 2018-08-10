using UnityEngine;

namespace Old.Entity.Modules
{
    public class MiscEventable : Module
    {
        public event KilledHandler KilledEntity;

        public void OnKilledEntity(KillEventArgs args)
        {
            if (KilledEntity != null)
                KilledEntity(args);
        }
    }
    public delegate void KilledHandler(KillEventArgs args);

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