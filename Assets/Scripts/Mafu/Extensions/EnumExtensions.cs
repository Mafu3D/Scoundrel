using System;

namespace Mafu.Extensions
{
    public static class EnumExtensions
    {
        public static T Next<T>(this T @enum) where T : Enum
        {
            T[] values = (T[])Enum.GetValues(typeof(T));
            int current = Array.IndexOf(values, @enum);
            int next = (current + 1) % values.Length;
            return values[next];
        }
    }
}