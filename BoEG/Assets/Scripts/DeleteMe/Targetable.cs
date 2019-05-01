namespace DeleteMe
{
    public class Targetable : ITargetable
    {
        public bool AttacksAllowed { get; }
        public bool SpellsAllowed { get; }
        
        public bool AllowAttack()
        {
            return AttacksAllowed;
        }

        public bool AllowSpell()
        {
            return SpellsAllowed;
        }
    }
    public interface ITargetable
    {
        bool AttacksAllowed { get; }
        bool SpellsAllowed { get; }
        bool AllowAttack();
        bool AllowSpell();               
    }
    
    
}