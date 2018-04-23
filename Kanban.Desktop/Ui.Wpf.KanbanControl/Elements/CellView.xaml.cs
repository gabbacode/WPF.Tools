using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ui.Wpf.KanbanControl.Elements
{
    /// <summary>
    /// codebehind for  CellView.xaml
    /// </summary>
    public partial class CellView
    {
        public CellView()
        {
            InitializeComponent();
        }

        private void ScrollViewerPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer && !e.Handled)
            {
                var scrollableHeight = ((ScrollViewer)sender).ScrollableHeight;
                if (scrollableHeight > 0)
                    // scroll current ScrollViewer and not scroll outer
                    return;

                // here current ScrollViewer no need to scroll so push event to outer
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = MouseWheelEvent;
                eventArg.Source = sender;

                var parent = ((Control)sender).Parent as UIElement;

                parent.RaiseEvent(eventArg);
            }
        }
    }
}
