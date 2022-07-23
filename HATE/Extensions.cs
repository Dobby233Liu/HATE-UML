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
            int n = selected.Count - 1;
            while (n >= 0)
            {
                int k = rng.Next(n + 1);
                swapFunc(selected[n], selected[k]);
                int idx = selected[k];
                selected[k] = selected[n];
                selected[n] = idx;
                n--;
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
            int n = selected.Count - 1;
            while (n >= 0)
            {
                int k = rng.Next(n + 1);
                swapFunc(selected[n], selected[k]);
                int idx = selected[k];
                selected[k] = selected[n];
                selected[n] = idx;
                n--;
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
        public static void SelectSome(this IList<int> list, float shufflechance, Random rng)
        {
            IList<int> listBak = new List<int>(list);
            list.Clear();
            foreach (int i in listBak)
                if (rng.NextDouble() <= shufflechance)
                    list.Add(i);
        }
    }
}
