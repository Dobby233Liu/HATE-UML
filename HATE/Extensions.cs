using System;
using System.Collections.Generic;

namespace HATE
{
    public static class Extensions
    {
        public static void Shuffle<T>(this IList<T> list, Random random)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void Shuffle<T>(this IList<T> list, Action<T, T> swapAction, Random random)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));

            int n = list.Count;
            while (n-- > 1)
            {
                int k = random.Next(n + 1);
                swapAction(list[n], list[k]);
            }
        }

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
    }
}
