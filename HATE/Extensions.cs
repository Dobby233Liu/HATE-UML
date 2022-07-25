using System;
using System.Collections;
using System.Collections.Generic;

namespace HATE;

public static class Extensions
{
    // from DeltaHATE.csx
    public static void ShuffleOnlySelected<T>(this IList<T> list, IList<int> selected, Action<int, int> swapFunc, Random rng)
    {
        int n = selected.Count - 1;
        while (n >= 0)
        {
            int k = rng.Next(n + 1);
            swapFunc(selected[n], selected[k]);
            (selected[k], selected[n]) = (selected[n], selected[k]);
            n--;
        }
    }
    public static void ShuffleOnlySelected(this IList list, IList<int> selected, Action<int, int> swapFunc, Random rng)
    {
        int n = selected.Count - 1;
        while (n >= 0)
        {
            int k = rng.Next(n + 1);
            swapFunc(selected[n], selected[k]);
            (selected[k], selected[n]) = (selected[n], selected[k]);
            n--;
        }
    }
    public static void ShuffleOnlySelected<T>(this Dictionary<T, T> list, IList<string> selected, Action<string, string> swapFunc, Random rng)
    {
        int n = selected.Count - 1;
        while (n >= 0)
        {
            int k = rng.Next(n + 1);
            swapFunc(selected[n], selected[k]);
            (selected[k], selected[n]) = (selected[n], selected[k]);
            n--;
        }
    }
    public static void SelectSome<T>(this IList<T> list, float shuffleChance, Random rng)
    {
        IList<T> listBak = new List<T>(list);
        list.Clear();
        foreach (var i in listBak)
            if (rng.NextDouble() <= shuffleChance)
                list.Add(i);
    }
}