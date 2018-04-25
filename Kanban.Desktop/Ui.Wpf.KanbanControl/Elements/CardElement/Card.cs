using System.Collections.Generic;
using System.Windows.Controls;

namespace Ui.Wpf.KanbanControl.Elements.CardElement
{
    public class Card
    {
        public Card(object item)
        {
            Item = item;
            View = new ContentControl();
            View.Content = this;
            View.DataContext = this;
        }

        public ContentControl View { get; private set; }

        public List<ActionItem> ActionItems { get; internal set; }

        public List<ContentItem> AdditionalContentItems { get; set; }

        public List<ContentItem> ShortContentItems { get; internal set; }

        public List<ContentItem> ContentItems { get; internal set; }

        public int VerticalCategoryIndex { get; internal set; }

        public int HorizontalCategoryIndex { get; internal set; }

        public object Item { get; private set; }
    }
}
