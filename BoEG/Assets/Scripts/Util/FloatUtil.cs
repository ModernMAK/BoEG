using UnityEngine;

namespace Util
{
    public static class FloatUtil
    {
        public static bool SafeEquals(this float value, float other)
        {
            return value.SafeEquals(other, Mathf.Epsilon);
        }

        public static bool SafeEquals(this float value, float other, float precision)
        {
            return -precision <= (value - other) && (value - other) <= precision;
        }
    }
}