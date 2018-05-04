using Data.Entities.Common.Redmine;

namespace Kanban.Desktop.Issues.Model
{
    public interface IIssueModel
    {
        Issue LoadOrCreate(int? issueId);
    }
}
