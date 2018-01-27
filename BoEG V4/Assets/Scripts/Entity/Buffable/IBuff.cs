using System;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff
{
}

public struct ValueBuffData
{
    public float Flat;
    public float AdditivePercentage;
    public float MultiplicativePercentage;
}

public static class ValueBuffDataExt
{
    public static ValueBuffData SimplifyToBuff<T>(this IEnumerable<T> enumerable, Func<T, ValueBuffData> getData)
        where T : IBuff
    {
        var ret = new ValueBuffData
        {
            Flat = 0f,
            AdditivePercentage = 0f,
            MultiplicativePercentage = 0f
        };
        foreach (var buff in enumerable)
        {
            var data = getData(buff);
            ret.Flat += data.Flat;
            ret.AdditivePercentage += data.AdditivePercentage;
            if (data.MultiplicativePercentage != 0f)
                Debug.LogWarning("Multiplicative not implimented!");
        }
        return ret;
    }

    public static float ApplyToValue(this ValueBuffData d, float v)
    {
        return
            d.Flat +
            d.AdditivePercentage * v +
            (1f + d.MultiplicativePercentage) * v;
    }
}

/*Buff types are...
    Additive
        Flat
        Percentage
    Multiplicative
        Percentage
     
*/