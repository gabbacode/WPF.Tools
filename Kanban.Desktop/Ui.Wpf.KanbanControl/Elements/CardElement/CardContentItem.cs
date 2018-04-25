namespace Ui.Wpf.KanbanControl.Elements.CardElement
{
    public class CardContentItem : ICardContentItem
    {
        public CardContentItem(string path, CardContentArea area = CardContentArea.Main)
        {
            Path = path;
            Area = area;
        }

        public string Path { get; }

        public CardContentArea Area { get; }
    }
}
