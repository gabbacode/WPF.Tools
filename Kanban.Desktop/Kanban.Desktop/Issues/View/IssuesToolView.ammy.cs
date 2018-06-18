using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.Issues.View
{
    public partial class IssuesToolView : IIssuesTool
    {
        public IssuesToolView()
        {
            InitializeComponent();
        }

        public IViewModel ViewModel { get; set; }

        public void Configure(UiShowOptions options)
        {
        }
    }
}
