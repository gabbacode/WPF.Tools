using System.Windows.Controls;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;
using Ui.Wpf.Demo.ViewModels;

namespace Ui.Wpf.Demo.Views
{
    /// <summary>
    /// Interaction logic for FlyoutDemoView.xaml
    /// </summary>
    public partial class FlyoutDemoView : UserControl, IView
    {
        public FlyoutDemoView(FlyoutDemoViewModel vm)
        {
            ViewModel = vm;
            DataContext = vm;

            InitializeComponent();
        }

        public void Configure(UiShowOptions options)
        {
            ViewModel.Title = options.Title;
        }

        public IViewModel ViewModel { get; set; }
    }
}