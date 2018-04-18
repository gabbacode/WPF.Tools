namespace Ui.Wpf.KanbanControl.Elements
{
    internal class Card
    {
        public Card(object item)
        {
            Item = item;
            View = new CardView();
            View.Content = Item;

        }

        public CardView View { get; private set; }

        public int VerticalCategoryIndex { get; internal set; }

        public int HorizontalCategoryIndex { get; internal set; }

        internal object Item { get; private set; }
    }
}
