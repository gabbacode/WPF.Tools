using Ui.Wpf.Common;

namespace Kanban.Desktop.LocalBase.Models
{
    public class WizardViewRequest : ViewRequest
    {
        public bool CreateNewFile { get; set; }
        public string Uri { get; set; }
    }

    public class BoardViewRequest : ViewRequest
    {
        public IScopeModel Scope { get; set; }
        public string SelectedBoardName { get; set; }
    }

}
