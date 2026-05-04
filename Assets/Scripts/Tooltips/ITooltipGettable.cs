namespace Project.UI.Tooltips
{
    public interface ITooltipGettable
    {
        public bool TryGetTooltipInformation(out string content, out string header);
    }

}
