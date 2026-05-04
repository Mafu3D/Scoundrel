using System.Collections;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using Mafu.Extensions;

namespace Project.UI.Tooltips
{
    public class TooltipTrigger : SerializedMonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Simple")]
        [SerializeField] string header;
        [SerializeField] string content;

        [Header("Getter")]
        [OdinSerialize] ITooltipGettable tooltipGetter;

        [Header("Delay")]
        [SerializeField] float delay = 0.2f;

        Coroutine delayRoutine;

        public void OnPointerEnter(PointerEventData eventData)
        {
            delayRoutine = StartCoroutine(ShowTooltipDelayRoutine());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (delayRoutine != null)
            {
                StopCoroutine(delayRoutine);
                delayRoutine = null;
            }
            TooltipSystem.Hide();
        }

        private IEnumerator ShowTooltipDelayRoutine()
        {
            yield return new WaitForSeconds(delay);
            Show();
        }

        private void Show()
        {
            string contentString = "";
            string headerString = "";
            if (tooltipGetter != null)
            {
                tooltipGetter.TryGetTooltipInformation(out contentString, out headerString);
            }
            if (!content.IsNullOrEmpty())
            {
                contentString = content;
            }
            if (!header.IsNullOrEmpty())
            {
                headerString = header;
            }

            if (!contentString.IsNullOrEmpty() || !headerString.IsNullOrEmpty())
            {
                TooltipSystem.Show(contentString, headerString);
            }
        }
    }
}
