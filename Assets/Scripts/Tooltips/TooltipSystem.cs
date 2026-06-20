using UnityEngine;

namespace Project.UI.Tooltips
{
    public class TooltipSystem : MonoBehaviour
    {
        private static TooltipSystem current;
        [SerializeField] TooltipCollectionView tooltipCollectionView;

        void Awake()
        {
            current = this;
        }

        public static void Show(TooltipCollection tooltipCollection)
        {
            current.tooltipCollectionView.RegisterTooltips(tooltipCollection);
            current.tooltipCollectionView.Show();
        }

        public static void Hide()
        {
            current.tooltipCollectionView.Hide();
            current.tooltipCollectionView.DeregisterTooltips();
        }

        public static void AnchorTooltipCollectionView(Transform anchor)
        {
            current.tooltipCollectionView.SetAnchor(anchor);
        }

        public static void DeanchorTooltipCollectionView()
        {
            current.tooltipCollectionView.SetAnchor(null);
        }
    }

}
