namespace Util
{
    public static class FlagUtil
    {
        public static bool HasBit(this byte value, int place)
        {
            var bit = (1 << place);
            return (value & bit) == bit;
        }
        public static byte SetBit(this byte value, int place)
        {
            var bit = (byte)(1 << place);
            return (byte)(value | bit);
        }

        public static bool HasBit(this int value, int place)
        {
            var bit = (1 << place);
            return (value & bit) == bit;
        }
        public static int SetBit(this int value, int place)
        {
            var bit = (1 << place);
            return (int)(value | bit);
        }
        
        public static bool HasBit(this uint value, int place)
        {
            var bit = (1 << place);
            return (value & bit) == bit;
        }
        public static uint SetBit(this uint value, int place)
        {
            var bit = (uint)(1 << place);
            return (uint)(value | bit);
        }
        
        public static bool HasBit(this long value, int place)
        {
            var bit = (1L << place);
            return (value & bit) == bit;
        }
        public static long SetBit(this long value, int place)
        {
            var bit = (long)(1 << place);
            return (long)(value | bit);
        }
        
        public static bool HasBit(this ulong value, int place)
        {
            var bit = (1UL << place);
            return (value & bit) == bit;
        }
        public static ulong SetBit(this ulong value, int place)
        {
            var bit = (ulong)(1 << place);
            return (ulong)(value | bit);
        }
    }
}