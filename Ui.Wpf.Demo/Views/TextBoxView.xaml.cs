using System.Windows.Controls;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;
using Ui.Wpf.Demo.ViewModels;

namespace Ui.Wpf.Demo.Views
{
    /// <summary>
    /// Interaction logic for TextBoxView.xaml
    /// </summary>
    public partial class TextBoxView : UserControl, IView
    {
        public TextBoxView(TextBoxViewModel vm)
        {
            ViewModel = vm;
            DataContext = vm;

            InitializeComponent();
        }

        public void Configure(UiShowOptions options)
        {
        }

        public IViewModel ViewModel { get; set; }
    }
}
