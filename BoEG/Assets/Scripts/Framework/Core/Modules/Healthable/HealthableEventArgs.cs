using System;

namespace Framework.Core.Modules
{
    public class HealthableEventArgs : EventArgs
    {
        public HealthableEventArgs(float change)
        {
            Change = change;
        }

        public float Change { get; private set; }
    }
}