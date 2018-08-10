using Core;

namespace Old.Entity.Modules.Armorable
{
    public class DamageEventArgs : System.EventArgs
    {
        public DamageEventArgs(Damage damage)
        {
            Damage = damage;
        }
        public Damage Damage { get; private set; }
    }
}