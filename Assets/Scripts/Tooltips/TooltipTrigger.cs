using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using Mafu.Extensions;

namespace Project.UI.Tooltips
{
    public abstract class TooltipTrigger : SerializedMonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Delay")]
        [SerializeField] private float delay = 0.2f;

        [Header("Anchor")]
        [SerializeField] private Transform anchor;
        [SerializeField] private bool forceAnchor = false;

        Coroutine delayRoutine;

        protected abstract TooltipCollection GetTooltipCollection();

        public void OnPointerEnter(PointerEventData eventData) => ShowTooltip();

        public void OnPointerExit(PointerEventData eventData) => HideTooltip();

        private void ShowTooltip()
        {
            TooltipCollection tooltipCollection = GetTooltipCollection();
            if (forceAnchor)
            {
                TooltipSystem.AnchorTooltipCollectionView(anchor);
            }
            delayRoutine = StartCoroutine(ShowTooltipDelayRoutine(tooltipCollection));
        }

        private IEnumerator ShowTooltipDelayRoutine(TooltipCollection tooltipCollection)
        {
            yield return new WaitForSeconds(delay);
            TooltipSystem.Show(tooltipCollection);
        }

        private void HideTooltip()
        {
            if (delayRoutine != null)
            {
                StopCoroutine(delayRoutine);
                delayRoutine = null;
            }
            TooltipSystem.Hide();
            TooltipSystem.DeanchorTooltipCollectionView();
        }
    }
}
