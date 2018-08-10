using System;

namespace Old.Entity.Modules.Magicable
{
    public class ManaModifiedEventArgs : EventArgs
    {
        public ManaModifiedEventArgs(float changeInMana)
        {
            ChangeInMana = changeInMana;
        }
        public float ChangeInMana { get; private set; }
    };
}