using UnityEngine;

namespace Modules.Magicable
{
    public class ManaModifiedEventArgs : EndgameEventArgs
    {
        public ManaModifiedEventArgs(GameObject source, GameObject owner, float modified) : base(source, owner)
        {
            Modified = modified;
        }

        public float Modified { get; private set; }
    }
}