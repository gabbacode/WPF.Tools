using MahApps.Metro.SimpleChildWindow;
using Ui.Wpf.Common.ShowOptions;

namespace Ui.Wpf.Common
{
    /// <summary>
    /// Interaction logic for ChildWindowView.xaml
    /// </summary>
    public partial class ChildWindowView : ChildWindow
    {
        public ChildWindowView(UiShowChildWindowOptions options)
        {
            DataContext = options;
            InitializeComponent();
        }
    }
}
