using UnityEngine;

namespace Mafu.Extensions
{
    public static class GameObjectExtensions
    {
        public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;
    }
}