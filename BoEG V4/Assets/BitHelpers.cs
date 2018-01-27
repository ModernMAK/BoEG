public static class BitHelpers
{
    //Starts at 1 (Follows Syncvar logic)
    public static bool GetBit(this int v, int i)
    {
        var mask = 1 << (i - 1);
        return (v & mask) == mask;
    }

    //Starts at 1 (Follows Syncvar logic)
    public static bool GetBit(this uint v, int i)
    {
        var mask = 1u << (i - 1);
        return (v & mask) == mask;
    }

    //Starts at 1 (Follows Syncvar logic)
    public static bool GetBit(this long v, int i)
    {
        var mask = 1L << (i - 1);
        return (v & mask) == mask;
    }
}