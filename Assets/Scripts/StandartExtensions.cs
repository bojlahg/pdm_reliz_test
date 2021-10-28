using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StandartExtensions
{
    static public T GetRandom<T>(this List<T> list)
    {
        if (list.Count > 0)
        {
            return list[Random.Range(0, list.Count)];
        }
        return default(T);
    }

    static public T GetRandom<T>(this System.Array arr)
    {
        if (arr.Length > 0)
        {
            return (T)arr.GetValue(Random.Range(0, arr.Length));
        }
        return default(T);
    }
}
