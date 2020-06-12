namespace Entity.Abilities.FlameWitch
{
    public class Overheat
    {
        /* Transformation Spell
         * Applies DOT around FlameWitch (Melee Range)
         * Upgrades FlameWitch's Abilities
         * On Death, deal damage to enemies in AOE.
         * Drains mana.
        */
    }

    public class Ignite
    {
        /* Unit-Target Spell
         * Applies DOT
         * Deals Damage on Cast
         *
         * When OverHeating;
         *     Eemy heroes in an AOE also recieve DOT
         */
    }

    public class Fireball
    {
        /* Ground-Taget Spell
         * Deals damage along path.
         * When OverHeating;
         *     Path is longer.
         */
    }

    public class FlashFire
    {
        /* Channeling Spell
         * Deals damage in an AOE around FlameWitch
         * Travels further based on channel duration.
         * When Overheating;
         *     Channel does not prevent movement.
         */
    }
}

namespace Entity.Abilities.WarpedMagi
{
    public class MagicalBacklash
    {
        /* Passive Spell
         * Units who cast spells in an AOE take damage based on manacost.
         */
    }

    public class WarpedBarrier
    {
        /* Unit Target Spell
         * Magical Damage taken is absorbed
         * % of Damage absorebed removes mana in an AOE
         */
    }

    public class UnstableMagic
    {
        /* Ground Target Spell
         * Magical Nuke, jumps between targets.
         * Does not target the same target twice. 
         */
    }

    public class MagicalInstability
    {
        /* Self-Target Spell
         * Negates Magical Damage.
         * Magical Damage grants mana for the duration.
         */
    }
}

namespace Entity.Abilities.LastHunter
{
    public class ReleasingTheHounds
    {
        /* No-Target
         * Summons a controllable pet
         */
    }

    public class HuntersStrike
    {
        /* Ground target
         * Blink To target
         * Deal damage to units between origin and target
         */
    }


    public class Perception
    {
        /* Unit Target
         * Target remains visible while in range indefinately.
         * Target remains visible for a short delay after leaving range.
         */
        
    }
    
}