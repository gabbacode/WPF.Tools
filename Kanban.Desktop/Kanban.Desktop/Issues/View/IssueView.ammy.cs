using Kanban.Desktop.Issues.ViewModel;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.Issues.View
{
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
