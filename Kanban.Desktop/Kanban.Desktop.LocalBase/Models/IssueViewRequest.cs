using Ui.Wpf.Common;

namespace Kanban.Desktop.LocalBase.Models
{
    public class IssueViewRequest : ViewRequest
    {
        public int? IssueId { get; set; }
    }
}
