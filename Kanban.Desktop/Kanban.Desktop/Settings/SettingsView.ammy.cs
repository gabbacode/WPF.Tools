using Ui.Wpf.Common;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.Settings
{
    /// <summary>
    /// codebehind of SettingsView.xaml
    /// </summary>
    public partial class SettingsView : ISettingsView
    {
        public SettingsView(ISettingsViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
            DataContext = viewModel;
        }

        public IViewModel ViewModel { get; set; }
        
        public void Configure(UiShowOptions options)
        {
            ViewModel.Title = options.Title;
        }
    }
}
