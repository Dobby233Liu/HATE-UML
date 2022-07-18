/*
    HATE-UML: The UNDERTALE corruptor, UndertaleModLib edition.
    Copyright (C) 2016 RedSpah
    Copyright (C) 2022 Dobby233Liu

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

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

        public static byte[] Garble(this byte[] array, float chnc, Random random)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));

            return array.Select(x => (char.IsLetterOrDigit((char)x) && random.NextDouble() < chnc)  ? (byte)(random.Next(75) + 47) : x).ToArray();         
        }
    }

}
