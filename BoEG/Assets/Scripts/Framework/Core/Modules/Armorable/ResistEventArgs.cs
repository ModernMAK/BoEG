using System;
using Framework.Types;

namespace Framework.Core.Modules
{
    public class ResistEventArgs : EventArgs
    {
        public ResistEventArgs(Damage before, Damage after)
        {
            Before = before;
            After = after;
        }

        public Damage Before { get; private set; }
        public Damage After { get; private set; }
    }
}