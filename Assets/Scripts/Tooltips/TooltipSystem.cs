using UnityEngine;

namespace Project.UI.Tooltips
{
    public class TooltipSystem : MonoBehaviour
    {
        private static TooltipSystem current;
        [SerializeField] Tooltip tooltip;

        void Awake()
        {
            current = this;
            current.tooltip.gameObject.SetActive(false);
        }

        public static void Show(string content, string header = "")
        {
            current.tooltip.SetText(content, header);
            current.tooltip.gameObject.SetActive(true);
        }

        public static void Hide()
        {
            current.tooltip.gameObject.SetActive(false);
        }
    }

}
