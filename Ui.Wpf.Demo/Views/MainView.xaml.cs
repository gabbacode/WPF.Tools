using System.Windows.Controls;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;
using Ui.Wpf.Demo.ViewModels;

namespace Ui.Wpf.Demo.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl, IView
    {
        public MainView(MainViewModel vm)
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