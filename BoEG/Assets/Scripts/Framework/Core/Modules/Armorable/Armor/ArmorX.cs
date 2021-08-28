namespace MobaGame.Framework.Core.Modules
{
    public static class ArmorX
    {

        public static float CalculateReduction(float value, float block=default, float resistance=default, bool immunity = false)
        {
            if (immunity)
                return value;

            //First apply block
            var blocked = block;

            //Avoids Block and resistance canceling out
            //If all damage is blocked, resistance (being applied after, has nothing to resist)
            if (value < block)
                blocked += resistance * (value - block);
            //If resistance makes us block everything, return the value, otherwise only return blocked
            return blocked > value ? value : blocked;
        }
    }
}