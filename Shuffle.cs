//https://qiita.com/o8que/items/bf07f824b3093e78d97a

using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
    }
}
