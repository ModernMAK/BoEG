using System;

namespace Entity.Abilities
{
    public sealed class TickActionDelegate : TickAction
    {
        public TickActionDelegate(TickData data, Action logic) : base(data)
        {
            Delegate = logic;
        }

        private Action Delegate { get; }

        protected override void Logic()
        {
            if (Delegate != null)
                Delegate();
        }
    }
}