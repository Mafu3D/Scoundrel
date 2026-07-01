using Sirenix.Serialization;
using UnityEngine;

namespace Project.UI.Tooltips
{
    public class RuntimeTooltipTrigger : TooltipTrigger
    {
        [Header("Runtime Getter")]
        [OdinSerialize] ITooltipGettable tooltipGetter;

        protected override TooltipCollection GetTooltipCollection()
        {
            if (tooltipGetter == null)
            {
                return default;
            }

            if (!tooltipGetter.TryGetTooltipInformation(out TooltipCollection tooltipCollection))
            {
                return default;
            }

            return tooltipCollection;
        }
    }
}
