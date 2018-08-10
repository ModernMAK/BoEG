using Core;
using UnityEngine;

namespace Old.Entity.Modules.Armorable
{
    public class ResistEventArgs : System.EventArgs
    {
        public ResistEventArgs(float resisted, DamageType type, GameObject source) : this(new Damage(resisted,type,source))
        {
        }
        public ResistEventArgs(Damage resisted)
        {
            Resisted = resisted;
        }
        public Damage Resisted { get; private set; }
    }
}