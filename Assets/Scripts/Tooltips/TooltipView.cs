using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Tooltips
{

    public class TooltipView : MonoBehaviour
    {
        [Header("Text Settings")]
        [SerializeField] private int characterWrapLimit;

        [Header("Components")]
        [SerializeField] private TextMeshProUGUI headerText;
        [SerializeField] private TextMeshProUGUI subtitleText;
        [SerializeField] private TextMeshProUGUI contentText;
        [SerializeField] private LayoutElement layoutElement;

        TooltipData tooltipData;

        public void RegisterTooltipData(TooltipData tooltipData)
        {
            headerText.gameObject.SetActive(!string.IsNullOrEmpty(tooltipData.Header));
            headerText.text = tooltipData.Header;

            subtitleText.gameObject.SetActive(!string.IsNullOrEmpty(tooltipData.Subtitle));
            subtitleText.text = tooltipData.Subtitle;

            contentText.text = tooltipData.Content;

            ResizeTooltip();
        }

        void Update()
        {
            ResizeTooltip();
        }

        private void ResizeTooltip()
        {
            int headerLength = headerText.text.Length;
            int subtitleLength = subtitleText.text.Length;
            int contentLength = contentText.text.Length;

            layoutElement.enabled =  new [] {headerLength, subtitleLength, contentLength}.Max() > characterWrapLimit;
        }
    }
}
