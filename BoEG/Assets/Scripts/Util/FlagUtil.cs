public static class FlagUtil
{
    public static bool HasBit(this byte value, int place)
    {
        var bit = (1 << place);
        return (value & bit) == bit;
    }

    public static bool HasBit(this int value, int place)
    {
        var bit = (1 << place);
        return (value & bit) == bit;
    }
}