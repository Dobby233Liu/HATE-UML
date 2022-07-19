using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}
