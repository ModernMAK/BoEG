using System;

namespace Old.Entity.Modules.Healthable
{
    public class HealthModifiedEventArgs : EventArgs
    {
        public HealthModifiedEventArgs(float changeInHealth)
        {
            ChangeInHealth = changeInHealth;
        }
        public float ChangeInHealth { get; private set; }
    };
}