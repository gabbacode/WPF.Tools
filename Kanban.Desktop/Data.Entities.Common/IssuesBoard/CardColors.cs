namespace Data.Entities.Common.IssuesBoard
{
    public class CardColors
    {
        public CardColors()
        {

        }

        public CardColors(string borderColor, string backgroundColor)
        {
            BorderColor = borderColor;
            BackgroundColor = backgroundColor;
        }

        public string BorderColor { get; set; }

        public string BackgroundColor { get; set; }
    }
}