﻿using UnityEngine;

namespace MobaGame.Framework.Utility
{
    public static class FloatUtil
    {
        public static bool SafeEquals(this float value, float other)
        {
            return value.SafeEquals(other, Mathf.Epsilon);
        }

        public static bool SafeEquals(this float value, float other, float precision)
        {
            return -precision <= value - other && value - other <= precision;
        }
    }
}