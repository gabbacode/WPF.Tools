using System.Windows;
using System.Windows.Input;
using Ui.Wpf.KanbanControl.Common;

namespace Ui.Wpf.KanbanControl.Elements
{
    public class ActionItem
    {
        public ActionItem(ContentItem contentItem)
        {
            ContentItem = contentItem;
            Action = new DelegateCommand((o) =>
            {
                if (contentItem.Visibility == Visibility.Visible)
                {
                    contentItem.Visibility = Visibility.Collapsed;
                }
                else
                {
                    contentItem.Visibility = Visibility.Visible;
                }
            });
        }

        public ICommand Action { get; }

        private ContentItem ContentItem;
    }
}