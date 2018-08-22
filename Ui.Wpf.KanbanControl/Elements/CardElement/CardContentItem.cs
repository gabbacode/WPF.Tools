namespace Ui.Wpf.KanbanControl.Elements.CardElement
{
    public class CardContentItem : ICardContentItem
    {
        public CardContentItem(string path, CardContentArea area = CardContentArea.Main)
        {
            ExpressionPath = path;
            Area = area;
        }

        public string ExpressionPath { get; }

        public CardContentArea Area { get; }
    }
}
