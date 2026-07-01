using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Project.UI.Tooltips
{

    public readonly struct TooltipData
    {
        public readonly string Header;
        public readonly string Subtitle;
        public readonly string Content;
        public readonly Sprite Sprite;

        public TooltipData(string header, string subtitle, string content, Sprite image)
        {
            Header = header;
            Subtitle = subtitle;
            Content = content;
            Sprite = image;
        }
    }

    public readonly struct TooltipCollection
    {
        public readonly List<TooltipData> TooltipDatas;

        public TooltipCollection(List<TooltipData> tooltipDatas)
        {
            TooltipDatas = tooltipDatas;
        }
    }
}
