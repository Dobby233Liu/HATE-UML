using System;
using System.Collections;
using System.Collections.Generic;

namespace HATE
{
    public static class Extensions
    {
        // from DeltaHATE.csx
        public static void ShuffleOnlySelected<T>(this IList<T> list, IList<int> selected, Action<int, int> swapFunc, Random rng)
        {
            int n = selected.Count;
            while (n > 1)
            {
                n--;

                int k = rng.Next(n + 1);
                swapFunc(selected[n], selected[k]);
                int idx = selected[k];
                selected[k] = selected[n];
                selected[n] = idx;
            }
        }
        public static void ShuffleOnlySelected<T>(this IList<T> list, IList<int> selected, Random rng)
        {
            list.ShuffleOnlySelected(selected, (n, k) =>
            {
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }, rng);
        }
        public static void ShuffleOnlySelected(this IList list, IList<int> selected, Action<int, int> swapFunc, Random rng)
        {
            int n = selected.Count;
            while (n > 1)
            {
                n--;

                int k = rng.Next(n + 1);
                swapFunc(selected[n], selected[k]);
                int idx = selected[k];
                selected[k] = selected[n];
                selected[n] = idx;
            }
        }
        public static void ShuffleOnlySelected(this IList list, IList<int> selected, Random rng)
        {
            list.ShuffleOnlySelected(selected, (n, k) =>
            {
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }, rng);
        }

        public static void SelectSome(this IList<int> list, float amountToKeep, Random rng)
        {
            int toRemove = (int)(list.Count * (1 - amountToKeep));
            for (int i = 0; i < toRemove; i++)
                list.RemoveAt(rng.Next(list.Count));
        }
    }
}
