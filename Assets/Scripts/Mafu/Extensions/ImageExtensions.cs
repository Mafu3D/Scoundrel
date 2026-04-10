using UnityEngine;
using UnityEngine.UI;

namespace Mafu.Extensions
{
    public static class ImageExtensions
    {
        public static void SetAlpha<T>(this T obj, float alpha) where T : Image
        {
            Color currentColor = obj.color;
            currentColor.a = alpha;
            obj.color = currentColor;
        }
    }
}