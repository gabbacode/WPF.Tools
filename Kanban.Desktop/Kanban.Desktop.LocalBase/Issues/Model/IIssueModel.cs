using Data.Entities.Common.Redmine;
using System.Threading.Tasks;

namespace Kanban.Desktop.LocalBase.Issues.Model
{
    public interface IIssueModel
    {
        Task<Issue> LoadOrCreateAsync(int? issueId);
    }
}