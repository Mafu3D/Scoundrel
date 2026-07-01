using UnityEngine;

namespace Project.UI.Tooltips
{
    public class SimpleTooltipTrigger : TooltipTrigger
    {
        [Header("Simple")]
        [SerializeField] string header;
        [SerializeField] string subtitle;
        [SerializeField] string content;
        [SerializeField] Sprite image;

        protected override TooltipCollection GetTooltipCollection()
        {
            return new(new(){ new(header, subtitle, content, image) });
        }
    }
}
