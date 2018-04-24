using System.Collections.Generic;

namespace Ui.Wpf.KanbanControl.Elements
{
    public class Card
    {
        public Card(object item)
        {
            Item = item;
            View = new CardView();
            View.Content = this;
            View.DataContext = this;
        }

        public CardView View { get; private set; }

        public List<ContentItem> ContentItems { get; internal set; }

        public int VerticalCategoryIndex { get; internal set; }

        public int HorizontalCategoryIndex { get; internal set; }

        public object Item { get; private set; }
    }
}
