using Kanban.Desktop.LocalBase.Issues.ViewModel;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.LocalBase.Issues.View
{
    /// <summary>
    /// Interaction logic for IssueView.xaml
    /// </summary>
    public partial class IssueView : IIssueView
    {
        public IssueView(IIssueViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;
        }

        public IViewModel ViewModel { get; set; }

        public void Configure(UiShowOptions options)
        {
        }
    }
}
