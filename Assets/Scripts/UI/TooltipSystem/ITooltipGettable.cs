namespace Project.UI.Tooltips
{
    public interface ITooltipGettable
    {
        public bool TryGetTooltipInformation(out TooltipCollection tooltipCollection);
    }

}
