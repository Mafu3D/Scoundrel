using System;
using System.Collections.Generic;
using System.Linq;

namespace Mafu.Extensions
{
    public static class ListExtensions
    {
        static Random rng;

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            if (rng == null) { rng = new Random(); }
            int count = list.Count;
            while (count > 1)
            {
                --count;
                int index = rng.Next(count + 1);
                (list[index], list[count]) = (list[count], list[index]);
            }

            return list;
        }

        /// <summary>
        /// Remove item from list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IList<T> Pop<T>(this IList<T> list, int index, out T item)
        {
            item = list[index];
            list.RemoveAt(index);
            return list;
        }

        /// <summary>
        /// Remove item from list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="newItem"></param>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IList<T> ReplaceAt<T>(this IList<T> list, T newItem, int index, out T item)
        {
            item = list[index];
            list[index] = newItem;
            return list;
        }

        public static void Swap<T>(this IList<T> list, int index1, int index2)
        {
            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }

        public static List<T> GetRandomElements<T>(this IEnumerable<T> list, int elementsCount)
        {
            return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
        }
    }
}