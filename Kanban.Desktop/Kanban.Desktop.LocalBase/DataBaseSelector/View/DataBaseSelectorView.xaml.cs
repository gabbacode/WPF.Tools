using Kanban.Desktop.LocalBase.DataBaseSelector.ViewModel;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.LocalBase.DataBaseSelector.View
{
    /// <summary>
    /// Interaction logic for DataBaseSelectorView.xaml
    /// </summary>
    public partial class DataBaseSelectorView : IBaseSelectorView
    {
        public DataBaseSelectorView(IBaseSelectorViewModel viewModel)
        {
            InitializeComponent();
            ViewModel   = viewModel;
            DataContext = ViewModel;
        }

        public IViewModel ViewModel { get; set; }
       

        public void Configure(UiShowOptions options)
        {
            ViewModel.Title = options.Title;
        }
    }
}
