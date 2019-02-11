using System.Windows.Controls;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;
using Ui.Wpf.Demo.ViewModels;

namespace Ui.Wpf.Demo.Views
{
    /// <summary>
    /// Interaction logic for ToolsView.xaml
    /// </summary>
    public partial class ToolsView : UserControl, IToolView
    {
        public ToolsView(ToolsViewModel vm)
        {
            ViewModel = vm;
            DataContext = vm;

            InitializeComponent();
        }

        public void Configure(UiShowOptions options)
        {
            ViewModel.Title = options.Title;
            ViewModel.CanClose = options.CanClose;
        }

        public IViewModel ViewModel { get; set; }
    }
}