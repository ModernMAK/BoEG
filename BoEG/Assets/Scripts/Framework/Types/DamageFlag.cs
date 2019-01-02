namespace Framework.Types
{
    [System.Flags]
    public enum DamageFlag
    {
        Attack = 1,
        Spell = 2, //To differentiate Attack / Spell Damage
        Reflected = 4, //To avoid loops for abilities which reflect damage
        
    }
}