namespace Ui.Wpf.KanbanControl.Elements
{
    public class CardItems : ICardItems
    {
        private string[] cardItems;

        public CardItems(string[] cardItems)
        {
            this.cardItems = cardItems;
        }
    }
}
