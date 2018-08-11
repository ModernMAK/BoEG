namespace Util
{
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
        public static bool HasBit(this uint value, int place)
        {
            var bit = (1 << place);
            return (value & bit) == bit;
        }
        public static bool HasBit(this long value, int place)
        {
            var bit = (1L << place);
            return (value & bit) == bit;
        }
        public static bool HasBit(this ulong value, int place)
        {
            var bit = (1UL << place);
            return (value & bit) == bit;
        }
    }
}