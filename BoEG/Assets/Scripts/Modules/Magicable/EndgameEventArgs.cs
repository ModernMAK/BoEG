using System;
using UnityEngine;

namespace Modules.Magicable
{
    public class EndgameEventArgs : EventArgs
    {
        public EndgameEventArgs(GameObject source, GameObject owner)
        {
            Source = source;
            Owner = owner;
        }

        /// <summary>
        /// The source of the Event
        /// </summary>
        public GameObject Source { get; private set; }

        /// <summary>
        /// The owner of the Event
        /// </summary>
        public GameObject Owner { get; private set; }
    }
}