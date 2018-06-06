using Data.Entities.Common.Redmine;
using System.Threading.Tasks;

namespace Kanban.Desktop.Issues.Model
{
    public interface IIssueModel
    {
        Task<Issue> LoadOrCreateAsync(int? issueId);
    }
}
