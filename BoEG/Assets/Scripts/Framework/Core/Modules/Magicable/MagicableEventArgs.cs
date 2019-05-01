using System;

namespace Framework.Core.Modules
{
    public class MagicableEventArgs : EventArgs
    {
        public MagicableEventArgs(float change)
        {
            Change = change;
        }

        public float Change { get; private set; }
    }
}