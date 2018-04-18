using System.Windows;

namespace Ui.Wpf.KanbanControl.Elements
{
    internal class Card
    {
        public Card(object item)
        {
            Item = item;
            View = new CardView();
        }

        public UIElement View { get; private set; }
        public int VerticalCategoryIndex { get; internal set; }
        public int HorizontalCategoryIndex { get; internal set; }

        private readonly object Item;
    }
}
