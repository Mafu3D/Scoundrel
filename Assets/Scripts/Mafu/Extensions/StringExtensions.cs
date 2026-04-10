namespace Mafu.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Converts the string to a FNV-1a hash.
        /// Useful for creating Diciontary keys instead of using strings.
        /// https://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
        /// </summary>
        /// <param name="str">Input string to hash.</param>
        /// <returns>An integer representing the FNV-1a hash of the input string.</returns>
        public static int ToFNV1aHash(this string str)
        {
            uint hash = 2166136261;
            foreach (char c in str)
            {
                hash = (hash ^ c) * 16777619;
            }
            return unchecked((int)hash);
        }
    }
}