using System;

namespace Networking
{
    public static class RandomExtensionMethods
    {
        public static ulong NextULong(this Random random)
        {
            return (ulong) random.NextLong();
        }

        public static long NextLong(this Random random)
        {
            var buffer = new byte[8];
            random.NextBytes(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}